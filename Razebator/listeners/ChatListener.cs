using HolyBot.Razebator.utils;
using McProtoNet.Core.Protocol;
using McProtoNet.Protocol340.Packets.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal class ChatListener : SessionListener {
        Bot client;

        public ChatListener(Bot client) { 
            this.client = client;
        }

        public void Dispose() {
            
        }

        public void onPacket(Packet p) {
            if (p is ServerChatMessagePacket p1) {
				BotU.log("msg: "+ StringU.formMsg(p1.Message));
                /*string[] ?command = messageToCommand(StringU.formMsg(p1.Message));
                if (command == null || command.Length == 0) {
                    return;
                } else {
					if (command[0].Equals("sayblock")) {
						int x = int.Parse(command[1]);
						int y = int.Parse(command[2]);
						int z = int.Parse(command[3]);
						client.chat(client.world.getBlock(x,y,z).displayName);
					}
                }*/
            }
        }

		public static String[] ?messageToCommand(string message) {
			try {
				//System.out.println(message); java moment
				bool sw = false;
				List<String> cmd = new List<String>();
				foreach (string pi in message.Split(" ")) {
					string piece = pi;
					if (sw) {
						cmd.Add(piece);
					} else if (piece.StartsWith(">>")) {//0.82 //0.83 //0.82
						sw = true;
						piece = piece.Replace(">>", "");//0.5 0.5 0.49 0.5
						cmd.Add(piece);
					} else if (piece.Contains(">>")) {
						sw = true;
						piece = piece.Substring(piece.IndexOf(">>")).Replace(">>", "");
						cmd.Add(piece);
					}
				}
				return cmd.ToArray<string>();
			} catch (Exception e) {
				//System.out.println(e);
				return null;
			}
		}
    }
}
