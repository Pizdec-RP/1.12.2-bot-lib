using HolyBot.Razebator.data;
using HolyBot.Razebator.math;
using McProtoNet.Protocol340.Data.World.Chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.level {
    internal class Block {
        public Vector3D pos;
        public int id;
        public string ?displayName;
        public string ?name;
        public double ?hardnes;
        public short ?stackSize;
        public AABB[] ?hitbox;
        public string ?material;
        public Dictionary<string, bool> ?harvestTools = new Dictionary<string, bool>();
        //public Dictionary<string, int> ?drops = new Dictionary<string, int>();
        public bool ?transparent;
        public double ?resistance;

        public int metadata;
        
        public Block(Vector3D pos) : this(pos, 0) {

        }
        public Block(Vector3D pos, int id) {
            this.id = id;
            this.pos = pos;
        }
        public Block(Vector3D pos, BlockState state) {
            this.pos = pos;
            this.id = state.Id;
            this.metadata = state.Data;
            DatamineBlock db = Level.blockList[id+"."+this.metadata];
            if (db != null) {
                this.displayName = db.displayName;
                this.name = db.name;
                this.hardnes = db.hardnes;
                this.stackSize = db.stackSize;
                this.hitbox = Block.hitboxFromId(id, metadata);
                this.material = db.material;
                this.harvestTools = db.harvestTools;
                this.transparent = db.transparent;
                this.resistance = db.resistance;
            } else {
                Console.WriteLine("ti eblan? id "+id+" ne bivaet, ti che ahuel?");
            }
        }

        public static AABB[] hitboxFromId(int id, int metadata) {
            List<AABB> list = new List<AABB>();



            return list.ToArray();
        }
    }
}
