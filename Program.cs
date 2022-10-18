using HolyBot.Razebator;
using HolyBot.Razebator.data;
using HolyBot.Razebator.level;
using HolyBot.Razebator.utils;
using McProtoNet.Protocol340.Data.World.Chunk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
namespace HolyBot;

class Program {
    static void Main(string[] args) {
        Level.load();


        //Level world = new Level();
        //Bot bot = new Razebator.Bot("tpa282","localhost:25565",world);
        //bot.connect();

        //Console.ReadLine();
    }
}
