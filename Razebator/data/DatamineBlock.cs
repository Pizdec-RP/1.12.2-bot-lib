using HolyBot.Razebator.math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.data {
    internal class DatamineBlock {
        public int id;
        public int metadata = 0;
        public string displayName;
        public string name;
        public double hardnes;
        public short stackSize;
        public AABB[] hitbox;
        public bool diggable = false;
        public string material;
        public Dictionary<string, bool> harvestTools = new Dictionary<string, bool>();
        //public Dictionary<string, int>? drops = new Dictionary<string, int>();
        public bool transparent;
        public double resistance;

        public DatamineBlock() {

        }

        public DatamineBlock clone() {
            DatamineBlock d = new DatamineBlock();
            d.id = id;
            d.metadata = metadata;
            d.displayName = displayName;
            d.name = name;
            d.hardnes = hardnes;
            d.stackSize = stackSize;
            d.hitbox = hitbox;
            d.diggable = diggable;
            d.material = material;
            d.harvestTools = new Dictionary<string, bool>(harvestTools);
            return d;
        }
    }
}
