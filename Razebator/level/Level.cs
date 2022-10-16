using HolyBot.Razebator.data;
using HolyBot.Razebator.math;
using HolyBot.Razebator.utils;
using McProtoNet.Geometry;
using McProtoNet.Protocol340.Data.World.Chunk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.level {
    internal class Level {
        public static Dictionary<McProtoNet.Protocol340.Data.World.Chunk.Block, DatamineBlock> blockList = new();// key = id + "." + metadata
        //JObject a;

        public Dictionary<ChunkCoordinates, ChunkColumn> columns = new Dictionary<ChunkCoordinates, ChunkColumn>();

        public static void load() {
            BotU.log("loading static world data...");
            string jsn = System.IO.File.ReadAllText(@".\datamine\blocks.json");
            JArray blocks = (JArray)JToken.Parse(jsn);
            //List<string> tl = new List<string>();
            foreach (JObject block in blocks) {
                if (block.ContainsKey("variations")) {
                    //try {
                        DatamineBlock skel = new DatamineBlock();
                        skel.id = block.GetValue("id").ToObject<ushort>();
                        skel.metadata = 0;
                        skel.displayName = block.GetValue("displayName").ToObject<string>();
                        skel.name = block.GetValue("name").ToObject<string>();
                        skel.hardnes = block.GetValue("hardness").ToObject<double>();
                        skel.stackSize = block.GetValue("stackSize").ToObject<short>();
                        skel.diggable = block.GetValue("diggable").ToObject<bool>();
                        skel.transparent = block.GetValue("transparent").ToObject<bool>();
                        skel.resistance = block.GetValue("resistance").ToObject<double>();

                        foreach (JObject variation in (JArray)block.GetValue("variations")) {
                            DatamineBlock temp = skel.clone();
                            int mn = 0;
                            //metadata, displayName, (description ignored)
                            if (variation.ContainsKey("displayName"))
                                temp.displayName = variation.GetValue("displayName").ToObject<string>();
                            
                            temp.hitbox = getMultipileHitbox(skel.name, mn, variation.Count);
                            blockList.Add(new McProtoNet.Protocol340.Data.World.Chunk.Block(temp.id,variation.GetValue("metadata").ToObject<byte>()), temp);
                            mn++;
                        }
                    //} catch (Exception ex) {
                    //    Console.WriteLine(ex.ToString());
                    //}
                } else {
                    try {
                        DatamineBlock temp = new DatamineBlock();
                        temp.id = block.GetValue("id").ToObject<ushort>();
                        temp.metadata = 0;
                        temp.displayName = block.GetValue("displayName").ToObject<string>();
                        temp.name = block.GetValue("name").ToObject<string>();
                        temp.hardnes = block.GetValue("hardness").ToObject<double>();
                        temp.stackSize = block.GetValue("stackSize").ToObject<short>();
                        temp.diggable = block.GetValue("diggable").ToObject<bool>();
                        temp.transparent = block.GetValue("transparent").ToObject<bool>();
                        temp.resistance = block.GetValue("resistance").ToObject<double>();
                        temp.hitbox = getSingleStateHitbox(temp.name);
                        blockList.Add(new McProtoNet.Protocol340.Data.World.Chunk.Block(temp.id,0), temp);
                    } catch (Exception e) {
                        Console.WriteLine(block.GetValue("id").ToObject<int>()+" is broken");
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            BotU.log("done");
        }

        public void setBlock(Point3_Int pos, ushort id, byte data) {
            try {
                int blockX = pos.X & 15;
                int blockY = pos.Y & 15;
                int blockZ = pos.Z & 15;

                int chunkX = pos.X >> 4;
                int chunkY = pos.Y >> 4;
                int chunkZ = pos.Z >> 4;
                columns[new ChunkCoordinates(chunkX, chunkZ)][chunkY][pos.X, pos.Y, pos.Z] = new McProtoNet.Protocol340.Data.World.Chunk.Block(id,data);//[new ChunkCoordinates(chunkX, chunkZ)].Chunks[chunkY].Storage[pos.X, pos.Y, pos.Z] = new BlockState(id, data);//.getChunks()[chunkY].set(blockX, blockY, blockZ, state);
            } catch (Exception e) {
                //e.printStackTrace();
            }
        }

        public Block getBlock(double x, double y, double z) {
            return this.getBlock(new Vector3D(x,y,z));
        }

        public Block getBlock(Vector3D pos) {
            /*if (pos.y < 0 || pos.y > 256) {
                return new Block(pos.floor(),0,0);
            }*/
            try {
                int bx = (int)Math.Floor(pos.getX()) & 15;
                int by = (int)Math.Floor(pos.getY()) & 15;
                int bz = (int)Math.Floor(pos.getZ()) & 15;
                int chunkX = (int)Math.Floor(pos.getX()) >> 4;
                int chunkY = (int)Math.Floor(pos.getY()) >> 4;
                int chunkZ = (int)Math.Floor(pos.getZ()) >> 4;
                ChunkColumn ?chunks = columns[new ChunkCoordinates(chunkX, chunkZ)];
                if (chunks == null) {
                    BotU.log("cant get block cx:"+chunkX+" cy:"+chunkY+" cz:"+chunkZ+" c:"+columns.ContainsKey(new ChunkCoordinates(chunkX, chunkZ)));
                    return new Block(pos);
                }
                Chunk cc = chunks[chunkY];
                if (cc == null)
                    return new Block(pos);
                McProtoNet.Protocol340.Data.World.Chunk.Block state = cc[bx, by, bz];
                return new Block(pos, state);
            } catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                return new Block(pos);
            }
        }








        public static string text = System.IO.File.ReadAllText(@".\datamine\colliders.json");
        public static JObject json = JObject.Parse(text);

        public static AABB[] getMultipileHitbox(string blockname, int metadatanum, int statescount) {
            if (JsonU.isItValueOf<int>(json["blocks"][blockname])) {// hbid = 1
                //Console.WriteLine("chck "+blockname);
                int hbid = json["blocks"][blockname].Value<int>();
                
                double[][] h = json["shapes"][hbid.ToString()].ToObject<double[][]>();
                if (h.Length == 0) {
                    return new AABB[0];
                }
                return new AABB[] { new AABB(h[0]) };
            } else if (JsonU.isItValueOf<int[]>(json["blocks"][blockname])) {// hbids = [1,2]
                int[] hbids = json["blocks"][blockname].ToObject<int[]>();
                int hitboxescount = hbids.Length;
                if (hitboxescount % statescount == 0) {
                    int delta = hitboxescount / statescount; //сколько чисел приходится на 1 тип (2)

                    int startunit = metadatanum * delta; //6
                    AABB[] toreturn = new AABB[delta];
                    for (int i = 0; i < delta; i++) {
                        try {
                            double[] ar = json["shapes"][startunit + i].ToObject<double[]>();
                            toreturn[i] = new AABB(ar);
                        } catch {
                            //Console.WriteLine("!");
                        }
                    }
                    return toreturn;
                } else {
                    //Console.WriteLine("polniy pizdec "+blockname);
                    return new AABB[] { new AABB(0, 0, 0, 1, 1, 1) };
                }
            } else {
                //Console.WriteLine("unknown value for " + blockname);
                return new AABB[] { new AABB(0, 0, 0, 1, 1, 1) };
            }
        }

        public static AABB[] getSingleStateHitbox(string blockname) {
            try {
                JToken bl = json["blocks"][blockname];
                JValue block = (JValue)bl;
                Int64 code = (Int64)block.Value;// code = 1
                JArray array = (JArray)json["shapes"]["" + code]; //jobj
                double[][] arr = array.ToObject<double[][]>(); //arr = [[0.0,0.0,0.0,1.0,1.0,1.0]]
                if (arr.Length == 0) {// arr = []
                    //Console.WriteLine("empty hitbox for " + blockname);
                    return new AABB[0];
                } else { //arr = [[0.0,0.0,0.0,1.0,1.0,1.0]]
                    string scode = code.ToString();// scode = "1"
                    if (arr.Length == 1) { //arr = [[0.0,0.0,0.0,1.0,1.0,1.0]]
                        //string key = scode + ".0"; // key = "1.0"
                        AABB value = new AABB(arr[0][0], arr[0][1], arr[0][2], arr[0][3], arr[0][4], arr[0][5]); //arr[0][0] = 0.0
                        return new AABB[] { value };
                        //Console.WriteLine("added normal hitbox");
                    } else { //arr = [[0.0,0.0,0.0,1.0,1.0,1.0][0.0,0.0,0.0,1.0,1.0,1.0]]
                        //Console.WriteLine("metadata > 1 for " + blockname);
                        int i = 0;
                        AABB[] ta = new AABB[arr.Length];  //arr.Length = 2
                        foreach (double[] a in arr) { //a = [0.0,0.0,0.0,1.0,1.0,1.0], a = [0.0,0.0,0.0,1.0,1.0,1.0]
                            //string key = scode + "." + i; // "1.0", "1.1"
                            AABB value = new AABB(a[0], a[1], a[2], a[3], a[4], a[5]);
                            ta[i] = value;
                            i++;
                        }
                        return ta;
                    }
                }
            } catch (Exception e) {
                //Console.WriteLine("replaced "+blockname+" to 1x1 hitbox");
                AABB r = new AABB(0, 0, 0, 1, 1, 1);
                if (blockname.Contains(blockname))
                    r.maxY = 1.5;
                return new AABB[] { r };
            }
        }
    }
}
