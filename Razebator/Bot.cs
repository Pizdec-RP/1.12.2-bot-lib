using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HolyBot.Razebator.data;
using HolyBot.Razebator.level;
using HolyBot.Razebator.listeners;
using HolyBot.Razebator.math;
using HolyBot.Razebator.utils;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using McProtoNet.Protocol340;
using McProtoNet.Protocol340.Data;
using McProtoNet.Protocol340.Packets.Client.Game;
using Starksoft.Net.Proxy;

namespace HolyBot.Razebator {
    internal class Bot : IDisposable {
        //main 
        public String name;
        //public Proxy proxy;
        public String host;
        public ushort port;


        //listeners
        public List<SessionListener> listeners = new List<SessionListener>();
        public List<BotControler> controlers = new List<BotControler>();//it roll you cunt

        //tick
        public Thread tickThread;
        static byte tickrate = 50;
        public bool running = false;
        public int tickCounter = 0;

        //session
        TcpClient tcpClient;
        IPacketReaderWriter client;
        ISession session;

        //ingame data
        public double posX=0, posY=0, posZ=0 ;
        public double velX=0, velY=0, velZ=0;
        public float pitch = 0, yaw = 0;//p-updown/y-leftright
        public bool onGround = true;
        public bool isHoldSlowdownItem = false;
        public double health = 20;
        public double food = 10;
        public double saturation = 5;
        public GameMode gamemode = GameMode.SURVIVAL;
        public Effects effects = new Effects();
        public EntityStorage entityStorage = new EntityStorage();
        public int id;//bot entity id
        public Guid uuid;
        public int dimension;
        public string worldType;

        //
        public PhysicsControler physics;
        public LivecycleControler rl;
        public ChatListener chatListener;
        public EntityListener entityListener;

        //world
        public Level world;


        public Bot(String name, String host, Level ?worldlink) {
            this.name = name;
            if (host.Contains(":")) {
                this.host = host.Split(":")[0];
                this.port = ushort.Parse(host.Split(":")[1]);
            } else {
                this.host = host;
                this.port = 25565;
            }
            if (worldlink == null) {
                world = new Level();
            } else {
                world = worldlink;
            }
            Console.WriteLine("bot created");
        }

        internal Level getWorld() {
            return world;
        }

        public void destroy() {
            running = false;
            tickThread.Interrupt();
            foreach (SessionListener listener in listeners) {
                listener.Dispose();
            }
            foreach (BotControler controler in controlers) {
                controler.Dispose();
            }
            this.Dispose();
        }

        public void reconnect() {

        }

        public void register() {
            send(new ClientChatPacket("/register "));
        }

        public void send(Packet p) {
            client.SendPacket(p);
        }

        public void chat(string s) {
            send(new ClientChatPacket(s));
        }

        public Bot connect() {


            if (isOnline()) return this;
            Console.WriteLine("connecting bot");
            tcpClient = new TcpClient(host, port);
            client = new PacketReaderWiter(tcpClient.Client);
            session = new Session340(client, this.name);
            
            listeners.Add(new DefaultListener(this));

            physics = new PhysicsControler(this);
            controlers.Add(physics);

            rl = new LivecycleControler(this);
            controlers.Add(rl);

            chatListener = new ChatListener(this);
            listeners.Add(chatListener);

            entityListener = new EntityListener(this);
            listeners.Add(entityListener);

            client.OnPacketReceived += (pm, packet) => {
                //Console.WriteLine("packet: " + packet.GetType().Name);
                foreach (SessionListener listener in listeners) {
                    listener.onPacket(packet);
                }
            };

            this.tickThread = new Thread(this.tickLoop);
            running = true;
            this.tickThread.Start();

            Console.WriteLine("bot connected");
            session.Login();
            return this;
        }

        public bool isOnline() {
            return running || (tcpClient != null && tcpClient.Connected) && client.Connected;
        }

        public void tickLoop() {
            int curcomp = 0;
            int needtocompensate = 0;
            while (true) {
                //Console.WriteLine("tick ready: "+this.physics.ready+" c:"+this.physics.chunkReceived+" p:"+this.physics.posReceived);
                if (!running)
                    break;
                long timeone = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (isOnline()) {
                    tick();
                    tickCounter++;
                    if (tickCounter >= 100)
                        tickCounter = 0;
                }
                long timetwo = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                int raznica = (int)(timetwo - timeone);
                if (needtocompensate > 5000) {
                    needtocompensate = 0;
                    //Console.WriteLine("client overloaded, skiped "+needtocompensate/tickrate+" ticks");
                }
                if (raznica > 0 && raznica < tickrate) {
                    curcomp = tickrate - raznica;
                    //Console.WriteLine("comp "+raznica+"ms");
                    if (needtocompensate <= 0) {
                        Thread.Sleep(curcomp);
                    } else {
                        needtocompensate -= curcomp;
                    }
                } else if (raznica == 0) {
                    if (needtocompensate <= 0) {
                        Thread.Sleep(tickrate);
                    } else {
                        needtocompensate -= tickrate;
                    }
                } else {
                    //Console.WriteLine("pass "+raznica+"ms");
                    needtocompensate += raznica - tickrate;
                }
            }
        }

        public void tick() {
            foreach (BotControler controller in controlers) {
                controller.tick();
            }
        }

        public void Dispose() {
            
        }

        /*public void setProxy(ProxyType pt, string host, ushort port) {
            this.proxy = new Proxy(host, port, pt);
        }*/

        public void addX(double i) {
            this.posX += i;
        }

        public void addY(double i) {
            this.posY += i;
        }

        public void addZ(double i) {
            this.posZ += i;
        }

        public void remX(double i) {
            this.posX -= i;
        }

        public void remY(double i) {
            this.posY -= i;
        }

        public void remZ(double i) {
            this.posZ -= i;
        }

        public double getPosX() {
            return posX;
        }

        public void setPosX(double posX) {
            this.posX = posX;
        }

        public double getPosY() {
            return posY;
        }

        public void setPosY(double posY) {
            this.posY = posY;
        }

        public double getPosZ() {
            return posZ;
        }

        public void setPosZ(double posZ) {
            this.posZ = posZ;
        }

        public String getHost() {
            return host;
        }

        public int getPort() {
            return port;
        }

        public float getYaw() {
            return yaw;
        }

        public void setYaw(float yaw) {
            this.yaw = yaw;
        }

        public float getPitch() {
            return pitch;
        }

        public void setPitch(float pitch) {
            this.pitch = pitch;
        }

        public void addYaw(float i) {
            if (yaw == 360) {
                yaw = 0;
            } else {
                yaw += i;
            }
        }

        public void addPitch(float i) {
            pitch += i;
        }

        public Vector3D getPosition() {
            return new Vector3D(this.posX, this.posY, this.posZ);
        }

        public Vector3D getPositionInt() {
            return new Vector3D((int)Math.Floor(this.posX), (int)Math.Floor(this.posY), (int)Math.Floor(this.posZ));
        }

        public Guid getUUID() {
            return uuid;
        }

        public void setUUID(Guid uUID) {
            uuid = uUID;
        }

        public int getId() {
            return id;
        }

        public void setId(int id) {
            this.id = id;
        }

        public bool isInLiquid() {
            foreach (Vector3D corner in getHitbox().getCorners()) {
                if (getWorld().getBlock(corner).isLiquid()) {
                    return true;
                }
            }
            return false;
        }

        public bool isInWater() {
            foreach (Vector3D corner in getHitbox().getCorners()) {
                if (getWorld().getBlock(corner).isWater()) {
                    return true;
                }
            }
            return false;
        }

        public bool isInLava() {
            foreach (Vector3D corner in getHitbox().getCorners()) {
                if (getWorld().getBlock(corner).isLava()) {
                    return true;
                }
            }
            return false;
        }

        public void setposto(Vector3D pos) {
            this.posX = pos.x;
            this.posY = pos.y;
            this.posZ = pos.z;
        }

        public void setposto(double x, double y, double z) {
            this.posX = x;
            this.posY = y;
            this.posZ = z;
        }

        public Vector3D getEyeLocation() {
            return getPositionInt().add(0.5, 1.75, 0.5);
        }

        public AABB getHitbox() {
            return new AABB(posX - 0.3, posY, posZ - 0.3, posX + 0.3, posY + 1.8, posZ + 0.3);
        }

        public AABB getHitbox(Vector3D a) {
            return new AABB(posX + a.x - 0.3, posY + a.y, posZ + a.z - 0.3, posX + a.x + 0.3, posY + a.y + 1.8, posZ + a.z + 0.3);
        }

        public AABB getHitbox(double x, double y, double z) {
            return new AABB(posX + x - 0.3, posY + y, posZ + z - 0.3, posX + x + 0.3, posY + y + 1.8, posZ + z + 0.3);
        }

        public double distance(Vector3D r) {
            return VectorUtils.sqrt(getPosition(), r);
        }

        public ChunkCoordinates GetChunkCoordinates() {
            return new ChunkCoordinates((int)Math.Floor(posX) >> 4, (int)Math.Floor(posZ) >> 4);
        }

        public void velAdd(double x, double y, double z) {
            this.velX += x;
            this.velY += y;
            this.velZ += z;
        }
    }
}
