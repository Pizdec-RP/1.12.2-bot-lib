using McProtoNet.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.math
{
    internal class Vector3D
    {
        public double x, y, z;

        public Vector3D(double x, double y, double z) { this.x = x; this.y = y; this.z = z; }

		public Vector3D(Vector3D vec) {
			this.x = vec.x;
			this.y = vec.y;
			this.z = vec.z;
		}

		public double getX() {
			return this.x;
		}

		public double getY() {
			return this.y;
		}

		public double getZ() {
			return this.z;
		}

		public double getPosX() {
			return this.x;
		}

		public double getPosY() {
			return this.y;
		}

		public double getPosZ() {
			return this.z;
		}

		public double getBlockX() {
			return this.x;
		}

		public double getBlockY() {
			return this.y;
		}

		public double getBlockZ() {
			return this.z;
		}

		public void setX(double x) {
			this.x = x;
		}

		public void setY(double y) {
			this.y = y;
		}

		public void setZ(double z) {
			this.z = z;
		}

		public void addX(double i) {
			this.x += i;
		}

		public void addY(double i) {
			this.y += i;
		}

		public void addZ(double i) {
			this.z += i;
		}

		public Vector3D floorXZ() {
			this.x = Math.Floor(x);
			this.z = Math.Floor(z);
			return this;
		}

		public void origin() {
			this.x = 0;
			this.y = 0;
			this.z = 0;
		}

		public Vector3D func_vf() {
			this.x = Math.Floor(x);
			this.y = Math.Floor(y);
			this.z = Math.Floor(z);
			return this;
		}

		public Vector3D clone() {
			return new Vector3D(this.x, this.y, this.z);
		}

		public Vector3D floor() {
			return this.clone().func_vf();
		}

		public Vector3D VecToInt() {
			return new Vector3D((int)Math.Floor(x), (int)Math.Floor(y), (int)Math.Floor(z));
		}

		public Vector3D add(Vector3D other) {
			return new Vector3D(x + other.x, y + other.y, z + other.z);
		}

		public Vector3D add(double x, double y, double z) {
			return new Vector3D(this.x + x, this.y + y, this.z + z);
		}

		public Vector3D subtract(Vector3D other) {
			return new Vector3D(x - other.x, y - other.y, z - other.z);
		}

		public Vector3D subtract(double x, double y, double z) {
			return new Vector3D(this.x - x, this.y - y, this.z - z);
		}

		public Vector3D multiply(int factor) {
			return new Vector3D(x * factor, y * factor, z * factor);
		}

		public Vector3D multiply(double factor) {
			return new Vector3D(x * factor, y * factor, z * factor);
		}

		public Vector3D divide(int divisor) {
			return new Vector3D(x / divisor, y / divisor, z / divisor);
		}

		public Vector3D divide(double divisor) {
			return new Vector3D(x / divisor, y / divisor, z / divisor);
		}

		public Vector3D abs() {
			return new Vector3D(Math.Abs(x), Math.Abs(y), Math.Abs(z));
		}

		public override String ToString() {
			return "x:" + x + " y:" + y + " z:" + z;
		}

        public override bool Equals(object? obj) {
            return obj is Vector3D d &&
                   x == d.x &&
                   y == d.y &&
                   z == d.z;
        }

        public override int GetHashCode() {
            return HashCode.Combine(x, y, z);
        }

		public Vector3D up() {
			return new Vector3D(x, y+1, z);
        }

		public Point3_Int translate() {
			return new Point3_Int((int)Math.Floor(x), (int)Math.Floor(y), (int)Math.Floor(z));

		}
    }
}
