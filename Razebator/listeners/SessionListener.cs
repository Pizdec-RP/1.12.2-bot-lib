using McProtoNet.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    interface SessionListener : IDisposable {
        public abstract void onPacket(Packet p);
    }
}
