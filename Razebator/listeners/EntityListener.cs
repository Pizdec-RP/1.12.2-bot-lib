using HolyBot.Razebator.data;
using McProtoNet.Core.Protocol;
using McProtoNet.Protocol340.Packets.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal class EntityListener : SessionListener {

        Bot client;

        public EntityListener(Bot client) {
            this.client = client;
        }

        public void Dispose() {
            
        }

        public void onPacket(Packet p) {
            if (p is ServerEntityPositionPacket p1) {
                Entity en = client.entityStorage.get(p1.ID);
                if (en != null) {
                    //+=move i vel
                }
            } else if (p is ServerSpawnPlayerPacket p2) {
                
            } else if (p is ServerSpawnEntityPacket p3) {

            }
        }
    }
}
