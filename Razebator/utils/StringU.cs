using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.utils {
    internal class StringU {
        private static Dictionary<string, string> trnslt = new Dictionary<String, String>() {
    	    {"й","j" },
            {"ц","tz"},
            {"у","y"},
            {"к","K"},
            {"е","e"},
            {"н","H"},
            {"г","G"},
            {"ш","sh"},
            {"щ","sh"},
            {"з","3"},
            {"х","x"},
            {"ъ",""},
            {"ф","f"},
            {"ы","i"},
            {"в","B"},
            {"а","a"},
            {"п","p"},
            {"р","P"},
            {"о","O"},
            {"л","L"},
            {"д","D"},
            {"ж","z"},
            {"э","3"},
            {"я","R"},
            {"ч","4"},
            {"с","C"},
            {"м","M"},
            {"и","N"},
            {"т","T"},
            {"ь","b"},
            {"б","b"},
            {"ю","u"},
        };
        public static string formMsg(string message) {
            BotU.log(message);
            JObject json = JObject.Parse(message);
            string s = ""; //= json.GetValue("with").ToArray<JObject>()[1].Value<string>;
            if (json.ContainsKey("with")) {
                JArray args = json.GetValue("with").ToObject<JArray>();
                for (int i = 0; i < args.Count; i++) {
                    if (JsonU.isItValueOf<JValue>(args[i])) {
                        s += ((JValue)args[i]).ToObject<string>();
                    
                    } else {
                        BotU.log(args[i].GetType().ToString());
                    }
                }
            } else if (json.ContainsKey("extra")) {
                foreach (JObject extrapart in (JArray)json.GetValue("extra")) {
                    if (extrapart.ContainsKey("text")) {
                        s += extrapart.GetValue("text").Value<string>();
                    }
                }
            }
            return s;
        }

        public static bool contains(List<String> list, String what) {
            foreach (String str in list) {
                if (str.Contains(what))
                    return true;
            }
            return false;
        }
    
        public static bool backwardContains(List<String> list, String what) {
            foreach (String str in list) {
                if (what.Contains(str))
                    return true;
            }
            return false;
        }
    
        public static String translit(String text) {
            foreach (KeyValuePair<string,string> entry in trnslt) {
                text = text.Replace(entry.Key, entry.Value);
            }
            return text;
        }
    
        public static String ticksToElapsedTime(int ticks) {
            int i = ticks / 20;
            int j = i / 60;
            i = i % 60;
            return i < 10 ? j + ":0" + i : j + ":" + i;
        }

        public static String RndLetter() {
            return "q w e r t y u i o p a s d f g h j k l z x c v b n m".Split(" ")[MathU.rnd(0, 25)];
        }

        public static String RndRuLetter() {
            return "й ц у к е н г ш щ з х ъ ф ы в а п р о л д ж э я ч с м и т ь б ю".Split(" ")[MathU.rnd(0, 31)];
        }
    }
}
