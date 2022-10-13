using HolyBot.Razebator.utils;
using McProtoNet.Protocol340.Data;
using McProtoNet.Protocol340.Packets.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal class LivecycleControler : BotControler {

        Bot client;
        public bool respawn = true;

        public LivecycleControler(Bot client) {
            this.client = client;
        }

        public void Dispose() {
            
        }

        public void tick() {
            if (client.isOnline()) {
                if (respawn && client.health <= 0 && client.tickCounter%5==0) {
                    client.send(new ClientRequestPacket(ClientRequest.RESPAWN));
                    BotU.log("respawning");
                    return;
                }
            }
        }
    }
}
