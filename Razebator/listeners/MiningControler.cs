using HolyBot.Razebator.math;
using HolyBot.Razebator.utils;
using McProtoNet.Protocol340.Data;
using McProtoNet.Protocol340.Packets.Client.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal class MiningControler : BotControler {
        public Bot client;
        public Vector3D ?target = new Vector3D(0,0,0);
        public int ticksToBreak = 0;
        public MineState ?state = MineState.END;

        public enum MineState {
            START, MINING, END
        }

        public MiningControler(Bot client) {
            this.client = client;
        }

        public void Dispose() {
            this.target = null;
            this.state = null;
        }

        public void tick() {
            if (state == null)
                return;
            else if (state == MineState.START) {

            } else if (state == MineState.MINING) {

            }
        }

        public void setup(Vector3D block) {
            target = block;
            state = MineState.START;
        }

        public void finishDigging() {
            //if (Main.debug) System.out.println("mining ended");
            client.send(new ClientPlayerActionPacket(PlayerAction.FINISH_DIGGING, target.translate(), VectorUtils.rbf(client, target.add(0.5, 0.5, 0.5))));
            state = MineState.END;
        }

        public void prepareitem() {

            //BlockData blockdata = Main.getMCData().blockData.get(pos.getBlock(client).id);
            /*List<materialsBreakTime> mtm = Main.getMCData().materialToolMultipliers.get(blockdata.material);
            if (mtm != null) {
                for (materialsBreakTime item : mtm) {
                    //System.out.println(client.getItemInHand().getId()+" != "+item.toolId);
                    if (client.getItemInHand().getId() == item.toolId)
                        return;
                }
                for (materialsBreakTime item : mtm) {
                    client.setToSlotInHotbarWithItemId(item.toolId);
                }
            }*/
        }
    }
}
