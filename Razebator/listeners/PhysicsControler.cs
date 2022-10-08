using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal class PhysicsControler : BotControler {
        public State state = State.waitForPosPacket;
        public Bot client;
        public int sleepticks = 0;

        public PhysicsControler(Bot client) {
            this.client = client;
        }

        public void Dispose() {
            
        }

        public void tick() {
            if (state == State.ready) {
                if (!client.isOnline()) {
                    state = State.waitForPosPacket;
                    return;
                }

                playerUpdate();
            }
        }

        public void playerUpdate() {
            if (sleepticks > 0) {
                sleepticks--;
                return;
            }
        }

        public void reset() {
            client.velX = 0;
            client.velY = 0;
            client.velZ = 0;
        }
    }

    public enum State {
        waitForPosPacket, waitForChunk, ready
    }
}
