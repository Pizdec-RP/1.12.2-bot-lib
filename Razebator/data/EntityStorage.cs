using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.data {
    internal class EntityStorage {

        public Dictionary<int, Entity> entities = new();

        public EntityStorage() {

        }

        public Entity get(int id) {
            return entities[id];
        }
    }
}
