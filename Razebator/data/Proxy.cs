using Starksoft.Net.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.data {
    internal class Proxy {
        public ProxyType proxyType = ProxyType.None;
        public string host = "";
        public ushort port = 0;

        public Proxy(string host, ushort port, ProxyType pt) {
            this.host = host;
            this.port = port;
            proxyType = pt;
        }

        public Proxy() {
            this.proxyType = ProxyType.None;
        }

        public Boolean isDirect() {
            return proxyType == ProxyType.None;
        }
    }
}
