using HolyBot.Razebator.math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.data {
    internal class AABB {
		public double minX;
		public double minY;
		public double minZ;
		public double maxX;
		public double maxY;
		public double maxZ;

		public AABB(double x0, double y0, double z0, double x1, double y1, double z1) {
			this.minX = x0;
			this.minY = y0;
			this.minZ = z0;
			this.maxX = x1;
			this.maxY = y1;
			this.maxZ = z1;
		}

		public AABB(double[] arr) {
			minX = arr[0];
			minY = arr[1];
			minZ = arr[2];
			maxX = arr[3];
			maxY = arr[4];
			maxZ = arr[5];
        }

		public AABB clone() {
			return new AABB(this.minX, this.minY, this.minZ, this.maxX, this.maxY, this.maxZ);
		}

		public AABB Floor() {
			this.minX = Math.Floor(this.minX);
			this.minY = Math.Floor(this.minY);
			this.minZ = Math.Floor(this.minZ);
			this.maxX = Math.Floor(this.maxX);
			this.maxY = Math.Floor(this.maxY);
			this.maxZ = Math.Floor(this.maxZ);
			return this;
		}

		public AABB grow(double x, double y, double z) {
			return new AABB(this.getMinX() - x, this.getMinY() - y, this.getMinZ() - z, this.getMaxX() + x, this.getMaxY() + y, this.getMaxZ() + z);
		}

		public List<Vector3D> getCorners() {
			List<Vector3D> c = new List<Vector3D>();
			
			c.Add(new Vector3D(minX, minY, minZ));
			c.Add(new Vector3D(minX, maxY, minZ));
			c.Add(new Vector3D(minX, minY, maxZ));
			c.Add(new Vector3D(minX, maxY, maxZ));

			c.Add(new Vector3D(maxX, minY, minZ));
			c.Add(new Vector3D(maxX, maxY, minZ));
			c.Add(new Vector3D(maxX, minY, maxZ));
			c.Add(new Vector3D(maxX, maxY, maxZ));
			return c;
		}

		public AABB extend(double dx, double dy, double dz) {
			if (dx < 0)
				this.minX += dx;
			else
				this.maxX += dx;

			if (dy < 0)
				this.minY += dy;
			else
				this.maxY += dy;

			if (dz < 0)
				this.minZ += dz;
			else
				this.maxZ += dz;

			return this;
		}

		public AABB offset(Vector3D a) {
			this.minX += a.x;
			this.minY += a.y;
			this.minZ += a.z;
			this.maxX += a.x;
			this.maxY += a.y;
			this.maxZ += a.z;
			return this;
		}

		public AABB contract(double x, double y, double z) {
			this.minX += x;
			this.minY += y;
			this.minZ += z;
			this.maxX -= x;
			this.maxY -= y;
			this.maxZ -= z;
			return this;
		}

		public AABB expand(double x, double y, double z) {
			this.minX -= x;
			this.minY -= y;
			this.minZ -= z;
			this.maxX += x;
			this.maxY += y;
			this.maxZ += z;
			return this;
		}

		public AABB offset(double x, double y, double z) {
			this.minX += x;
			this.minY += y;
			this.minZ += z;
			this.maxX += x;
			this.maxY += y;
			this.maxZ += z;
			return this;
		}

		public double computeOffsetX(AABB other, double offsetX) {
			if (other.maxY > this.minY && other.minY < this.maxY && other.maxZ > this.minZ && other.minZ < this.maxZ) {
				if (offsetX > 0.0 && other.maxX <= this.minX) {
					offsetX = Math.Min(this.minX - other.maxX, offsetX);
				} else if (offsetX < 0.0 && other.minX >= this.maxX) {
					offsetX = Math.Max(this.maxX - other.minX, offsetX);
				}
			}
			return offsetX;
		}

		public double computeOffsetY(AABB other, double offsetY) {
			if (other.maxY > this.minY && other.minY < this.maxY && other.maxZ > this.minZ && other.minZ < this.maxZ) {
				if (offsetY > 0.0 && other.maxX <= this.minX) {
					offsetY = Math.Min(this.minX - other.maxX, offsetY);
				} else if (offsetY < 0.0 && other.minX >= this.maxX) {
					offsetY = Math.Max(this.maxX - other.minX, offsetY);
				}
			}
			return offsetY;
		}

		public double computeOffsetZ(AABB other, double offsetZ) {
			if (other.maxY > this.minY && other.minY < this.maxY && other.maxZ > this.minZ && other.minZ < this.maxZ) {
				if (offsetZ > 0.0 && other.maxX <= this.minX) {
					offsetZ = Math.Min(this.minX - other.maxX, offsetZ);
				} else if (offsetZ < 0.0 && other.minX >= this.maxX) {
					offsetZ = Math.Max(this.maxX - other.minX, offsetZ);
				}
			}
			return offsetZ;
		}

		public bool collide(AABB other) {
			return this.minX < other.maxX && this.maxX > other.minX &&
				   this.minY < other.maxY && this.maxY > other.minY &&
				   this.minZ < other.maxZ && this.maxZ > other.minZ;
		}

		public double getMinX() {
			return minX;
		}

		public void setMinX(double MinX) {
			this.minX = MinX;
		}

		public double getMinY() {
			return minY;
		}

		public void setMinY(double MinY) {
			this.minY = MinY;
		}

		public double getMinZ() {
			return minZ;
		}

		public void setMinZ(double MinZ) {
			this.minZ = MinZ;
		}

		public double getMaxX() {
			return maxX;
		}

		public void setMaxX(double MaxX) {
			this.maxX = MaxX;
		}

		public double getMaxY() {
			return maxY;
		}

		public void setMaxY(double MaxY) {
			this.maxY = MaxY;
		}

		public double getMaxZ() {
			return maxZ;
		}

		public void setMaxZ(double MaxZ) {
			this.maxZ = MaxZ;
		}

		public AABB setBB(AABB bb) {
			this.setMinX(bb.getMinX());
			this.setMinY(bb.getMinY());
			this.setMinZ(bb.getMinZ());
			this.setMaxX(bb.getMaxX());
			this.setMaxY(bb.getMaxY());
			this.setMaxZ(bb.getMaxZ());
			return this;
		}

		public double calculateXOffset(AABB bb, double x) {
			if (bb.getMaxY() <= this.getMinY() || bb.getMinY() >= this.getMaxY()) {
				return x;
			}
			if (bb.getMaxZ() <= this.getMinZ() || bb.getMinZ() >= this.getMaxZ()) {
				return x;
			}
			if (x > 0 && bb.getMaxX() <= this.getMinX()) {
				double x1 = this.getMinX() - bb.getMaxX();
				if (x1 < x) {
					x = x1;
				}
			}
			if (x < 0 && bb.getMinX() >= this.getMaxX()) {
				double x2 = this.getMaxX() - bb.getMinX();
				if (x2 > x) {
					x = x2;
				}
			}

			return x;
		}

		public double calculateYOffset(AABB bb, double y) {
			if (bb.getMaxX() <= this.getMinX() || bb.getMinX() >= this.getMaxX()) {
				return y;
			}
			if (bb.getMaxZ() <= this.getMinZ() || bb.getMinZ() >= this.getMaxZ()) {
				return y;
			}
			if (y > 0 && bb.getMaxY() <= this.getMinY()) {
				double y1 = this.getMinY() - bb.getMaxY();
				if (y1 < y) {
					y = y1;
				}
			}
			if (y < 0 && bb.getMinY() >= this.getMaxY()) {
				double y2 = this.getMaxY() - bb.getMinY();
				if (y2 > y) {
					y = y2;
				}
			}

			return y;
		}

		public double calculateZOffset(AABB bb, double z) {
			if (bb.getMaxX() <= this.getMinX() || bb.getMinX() >= this.getMaxX()) {
				return z;
			}
			if (bb.getMaxY() <= this.getMinY() || bb.getMinY() >= this.getMaxY()) {
				return z;
			}
			if (z > 0 && bb.getMaxZ() <= this.getMinZ()) {
				double z1 = this.getMinZ() - bb.getMaxZ();
				if (z1 < z) {
					z = z1;
				}
			}
			if (z < 0 && bb.getMinZ() >= this.getMaxZ()) {
				double z2 = this.getMaxZ() - bb.getMinZ();
				if (z2 > z) {
					z = z2;
				}
			}

			return z;
		}


		public override String ToString() {
			return "AABB [MinX=" + minX + ", MinY=" + minY + ", MinZ=" + minZ + ", MaxX=" + maxX + ", MaxY=" + maxY + ", MaxZ=" + maxZ + "]";
		}

        public AABB AddCoord(double x, double y, double z) {
			double MinX = this.getMinX();
			double MinY = this.getMinY();
			double MinZ = this.getMinZ();
			double MaxX = this.getMaxX();
			double MaxY = this.getMaxY();
			double MaxZ = this.getMaxZ();

			if (x < 0)
				MinX += x;
			if (x > 0)
				MaxX += x;

			if (y < 0)
				MinY += y;
			if (y > 0)
				MaxY += y;

			if (z < 0)
				MinZ += z;
			if (z > 0)
				MaxZ += z;

			return new AABB(MinX, MinY, MinZ, MaxX, MaxY, MaxZ);
		}

		public AABB setBounds(double MinX, double MinY, double MinZ, double MaxX, double MaxY, double MaxZ) {
			this.setMinX(MinX);
			this.setMinY(MinY);
			this.setMinZ(MinZ);
			this.setMaxX(MaxX);
			this.setMaxY(MaxY);
			this.setMaxZ(MaxZ);
			return this;
		}
	}
}
