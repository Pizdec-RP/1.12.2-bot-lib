using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.utils {
    internal class StringU {
        private static Dictionary<String, String> trnslt = new Dictionary<String, String>(); /*{
           
	    {
    	    Put("й","N");
            put("ц","|_|,");
            put("у","y");
            put("к","K");
            put("е","e");
            put("н","H");
            put("г","G");
            put("ш","|_|_|");
            put("щ","|_|_|");
            put("з","3");
            put("х","x");
            put("ъ","|o");
            put("ф","o|o");
            put("ы","|o |");
            put("в","B");
            put("а","a");
            put("п","p");
            put("р","P");
            put("о","O");
            put("л","L");
            put("д","D");
            put("ж","|-|-|");
            put("э","3");
            put("я","R");
            put("ч","4");
            put("с","C");
            put("м","M");
            put("и","N");
            put("т","T");
            put("ь","|o");
            put("б","b");
            put("ю","|-0");
        }
    };*/
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

        /*public static String componentToString(Component smth) {
            //System.out.println(packet.getMessage().toString());
            StringBuilder message = new StringBuilder();
            if (smth instanceof TranslatableComponent) {
            TranslatableComponent a = (TranslatableComponent)smth;
            if (!a.args().isEmpty()) {
                for (Component arg : a.args()) {
                    if (arg instanceof TextComponent) {
                        message.append(" ").append(((TextComponent)arg).content());
                    }
                }
            }
            if (!a.children().isEmpty()) {
                for (Component chl : a.children()) {
                    if (chl instanceof TextComponent) {
                    message.append(" ").append(((TextComponent)chl).content());
                }
		    } else if (smth instanceof TextComponent) {
                TextComponent a = (TextComponent)smth;
                message.append((a).content());
                if (!a.children().isEmpty()) {
                    for (Component chl : a.children()) {
                        if (chl instanceof TextComponent) {
                        message.append(" ").append(((TextComponent)chl).content());
                    }
                }
            }
		    return message.toString();
	    }*/

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
            //foreach (entry in trnslt) {
                //text = text.replace(entry.getKey(), entry.getValue());
            //}
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
