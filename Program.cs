using HolyBot.Razebator;
using HolyBot.Razebator.data;
using HolyBot.Razebator.level;
using HolyBot.Razebator.utils;
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


        //string[] fileEntries = Directory.GetFiles("../datamine");
        //foreach (string fileName in fileEntries)
        //    Console.WriteLine(fileName);
        //Level world = new Level();
        //Bot bot = new Razebator.Bot("tpa282","localhost:25565",world);
        //bot.connect();

        //Console.ReadLine();
    }
}
