using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.utils {
    internal class JsonU {
        public static bool isItValueOf<T> (JToken o) {
            try {
                o.ToObject<T>();
                return true;
            } catch (Exception e) {
                return false;
            }
        }
    }
}
