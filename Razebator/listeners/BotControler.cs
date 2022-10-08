using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.listeners {
    internal interface BotControler : IDisposable{
        public abstract void tick();
    }
}
