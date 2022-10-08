using McProtoNet.Core.Protocol;
using McProtoNet.Protocol340.Packets.Client.Game;
using McProtoNet.Protocol340.Packets.Server;
using McProtoNet.Protocol340.Data;
using McProtoNet.Protocol340.Data.World.Chunk;
using McProtoNet.Protocol340.Data.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolyBot.Razebator.level;

namespace HolyBot.Razebator.listeners {
    internal class DefaultListener : SessionListener {
        private Bot client;

        public DefaultListener(Bot bot) {
            this.client = bot;
        }

        public void Dispose() {
            
        }
        
        public void onPacket(Packet packet) {
            if (packet is ServerJoinGamePacket p) {
                //renderDist
                client.setId(p.EntityId);
                //physics sleep
                //wait for chunks and start tickloop
                //connected = true;
                client.gamemode = p.GameMode;
                client.send(new ClientSettingsPacket(
                    "ru",
                    (byte) 5,
                    ChatVisibility.FULL,
                    false,
                    new List<SkinPart>() { 
                        SkinPart.JACKET,
                        SkinPart.LEFT_PANTS_LEG,
                        SkinPart.HAT,
                        SkinPart.RIGHT_PANTS_LEG,
                        SkinPart.CAPE,
                        SkinPart.LEFT_SLEEVE,
                        SkinPart.RIGHT_SLEEVE
                    },
                    HandPreference.RIGHT_HAND
                ));
                //client.send(new ClientPluginMessagePacket("minecraft:brand", new byte[] { 7, 118, 97, 110, 105, 108, 108, 97 }));
            } else if (packet is ServerPlayerPositionRotationPacket p1) {
                client.send(new ClientTeleportConfirmPacket(p1.TeleportId));
                client.physics.sleepticks += 2;
                client.setPosX(p1.X);
                client.setPosY(p1.Y);
                client.setPosZ(p1.Z);
                //client.pm.before = new Vector3D(packet.getX(), packet.getY(), packet.getZ());
                client.setYaw(p1.Yaw);
                client.setPitch(p1.Pitch);
                //client.pm.beforePitch = packet.getPitch();
                //client.pm.beforeYaw = packet.getYaw();
                // BotU.log("pos packet received x:"+packet.getX()+" y:"+packet.getY()+" z:"+packet.getZ()+" yaw:"+packet.getYaw()+" pitch:"+packet.getPitch());
                client.send(new ClientPlayerPositionRotationPacket(client.getPosX(), client.getPosY(), client.getPosZ(), client.getYaw(), client.getPitch(), client.onGround));
                //client request packet with stats
                client.physics.reset();

                //Close window if window open block sqrt > 8
                /*if (client.crafter.windowType != null) {
                    if (VectorUtils.sqrt(client.crafter.lastWindowPos, new Vector3D(packet.getX(), packet.getY(), packet.getZ())) > 8) {
                        client.getSession().send(new ClientCloseWindowPacket(client.playerInventory.currentWindowId));
                        client.crafter.windowType = null;
                        client.playerInventory.currentWindowId = 0;
                    }
                }*/
            } else if (packet is ServerPlayerHealthPacket p3) {
                client.health = p3.Health;
                client.food = p3.Food;
                client.saturation = p3.Saturation;
                if (client.health <= 0) {
                    //end pvp
                    //block break manager stop & reset
                    //pathfinder stop & reset
                    //respawn packet send every tick while hp < 0
                }
            } else if (packet is ServerMultiBlockChangePacket p4) {
                foreach (BlockChangeRecord change in p4.Records) {
                    client.world.setBlock(change.Position, change.State.Id, change.State.Data);
                }
            } else if (packet is ServerUnloadChunkPacket p5) {
                // pohuy
            } else if (packet is ServerChunkDataPacket p6) {
                Column col = p6.Column;
                ChunkCoordinates cc = new ChunkCoordinates(col.X, col.Z);
                client.world.columns.Add(cc,col);
                if (cc.Equals(client.GetChunkCoordinates())) {
                    client.running = true;
                }
            } else if (packet is ServerBlockChangePacket p7) {
                client.world.setBlock(p7.Record.Position, p7.Record.State.Id, p7.Record.State.Data);
            } else if (packet is LoginSuccessPacket p8) {
                client.setUUID(p8.UUID);
            } else if (packet is ServerPlayerListEntryPacket p9) {
                //zdelay suka uzhe
            } else if (packet is ServerResourcePackSendPacket p10) {
                client.send(new ClientResourcePackStatusPacket(ResourcePackStatus.ACCEPTED));
                client.send(new ClientResourcePackStatusPacket(ResourcePackStatus.SUCCESSFULLY_LOADED));
            }
        }
    }
}
