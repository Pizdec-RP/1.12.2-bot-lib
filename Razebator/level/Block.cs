using HolyBot.Razebator.data;
using HolyBot.Razebator.math;
using HolyBot.Razebator.utils;
using McProtoNet.Protocol340.Data.World.Chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.level {
    internal class Block {
        public Vector3D pos;
        public ushort id;
        public string displayName;
        public string name;
        public double hardnes;
        public short stackSize;
        public AABB[] hitbox;
        public string material;
        public Dictionary<string, bool> harvestTools = new Dictionary<string, bool>();
        //public Dictionary<string, int> ?drops = new Dictionary<string, int>();
        public bool transparent;
        public double resistance;

        public byte metadata;
        
        public Block(Vector3D pos) : this(pos, new McProtoNet.Protocol340.Data.World.Chunk.Block(0,0)) {

        }
        public Block(Vector3D pos, McProtoNet.Protocol340.Data.World.Chunk.Block state) {
            this.pos = pos;
            this.id = state.Id;
            this.metadata = state.Data;
            DatamineBlock db = Level.blockList[new McProtoNet.Protocol340.Data.World.Chunk.Block(id,this.metadata)];
            if (db != null) {
                this.displayName = db.displayName;
                this.name = db.name;
                this.hardnes = db.hardnes;
                this.stackSize = db.stackSize;
                this.hitbox = new AABB[db.hitbox.Length];
                int i = 0;
                foreach (AABB h in db.hitbox) {
                    this.hitbox[i] = h.clone();
                    i++;
                }
                this.material = db.material;
                this.harvestTools = db.harvestTools;
                this.transparent = db.transparent;
                this.resistance = db.resistance;
            } else {
                Console.WriteLine("ti eblan? id "+state.ToString()+" ne bivaet, ti che ahuel?");
            }
        }


        public float getfriction() {
            if (id == 165) {//slime
                return 0.8F;
            } else if (id == 174) {//packed ice
                return 0.98F;
            } else if (id == 79) {//ice
                return 0.98F;
            } else if (id == 212) {//frosted ice
                return 0.98F;
            } else {
                return 0.6F;
            }
        }

        public AABB[] getHitbox() {
            return hitbox;
        }

        public bool collide(AABB hb) {
            foreach (AABB h in hitbox) {
                if (h.collide(hb))
                    return true;
            }
            return false;
        }

        public bool isLiquid() {
            return name.Contains("water") | name.Contains("lava");
        }

        public bool isWater() {
            return name.Contains("water");
        }

        public bool isLava() {
            return name.Contains("lava");
        }

        public double minY() {
            double d = double.MaxValue;
            foreach (AABB h in hitbox) {
                if (h.minY < d)
                    d = h.minY;
            }
            if (d == Double.MaxValue)
                return pos.y;
            return d;
        }

        public bool isAvoid() {
            return hitbox.Length == 0;
        }

        public double maxY() {
            double d = double.MinValue;
            foreach (AABB h in hitbox) {
                if (h.maxY > d)
                    d = h.maxY;
            }
            if (d == Double.MinValue)
                return pos.y;
            return d;
        }
    }
}
