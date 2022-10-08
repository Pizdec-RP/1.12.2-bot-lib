using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.level {
    internal class ChunkCoordinates {
		private int chunkX;
		private int chunkZ;

		public ChunkCoordinates(int chunkX, int chunkZ) {
			this.chunkX = chunkX;
			this.chunkZ = chunkZ;
		}

		public int getChunkX() {
			return chunkX;
		}

		public int getChunkZ() {
			return chunkZ;
		}

        public override bool Equals(object? obj) {
			if (!(obj is ChunkCoordinates)) {
				return false;
			}
			ChunkCoordinates coordsObj = (ChunkCoordinates)obj;
			if (coordsObj.getChunkX() == chunkX && coordsObj.getChunkZ() == chunkZ) {
				return true;
			} else {
				return false;
			}
		}

        public override int GetHashCode() {
            return HashCode.Combine(chunkX, chunkZ);
        }
    }
}
