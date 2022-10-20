using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolyBot.Razebator.math;
using McProtoNet.Core;

namespace HolyBot.Razebator.utils {
    internal class VectorUtils {
        public static Vector3D getDirection(double yaw, double pitch) {
            Vector3D vector = new Vector3D(0, 0, 0);
            double rotX = yaw;
            double rotY = pitch;
            vector.setY(-Math.Sin(MathU.toRadians(rotY)));
            double xz = Math.Cos(MathU.toRadians(rotY));
            vector.setX(-xz * Math.Sin(MathU.toRadians(rotX)));
            vector.setZ(xz * Math.Cos(MathU.toRadians(rotX)));
            return vector;
        }

        public static Vector3D vector(float Yaw, float Pitch, double speed, Bot client, bool addY) {
            Vector3D vector = new Vector3D(0, 0, 0);
            double rotX = Yaw;
            double rotY = Pitch;

            double xz = Math.Cos(MathU.toRadians(rotY));
            if (addY)
                vector.setY(-Math.Sin(MathU.toRadians(rotY)));//0.35
            else
                xz = 1;
            vector.setX(-xz * Math.Sin(MathU.toRadians(rotX)));
            vector.setZ(xz * Math.Cos(MathU.toRadians(rotX)));
            vector = vector.multiply(speed);
            return vector;
        }

        public static bool equalsInt(Vector3D one, Vector3D two) {
            if (one == null || two == null)
                return false;
            if ((int)Math.Floor(one.getX()) == (int)Math.Floor(two.getX()) && (int)Math.Floor(one.getY()) == (int)Math.Floor(two.getY()) && (int)Math.Floor(one.getZ()) == (int)Math.Floor(two.getZ()))
                return true;
            return false;
        }

        public static Vector3D getVector(Vector3D from, Vector3D to) {
            return new Vector3D(MathU.getDir(to.x - from.x), MathU.getDir(to.y - from.y), MathU.getDir(to.z - from.z));
        }

        public static double sqrt(Vector3D one, Vector3D two) {
            double distance = Math.Sqrt(Math.Pow(one.getX() - two.getX(), 2) + Math.Pow(one.getY() - two.getY(), 2) + Math.Pow(one.getZ() - two.getZ(), 2));
            return distance;
        }

        public static double sqrt2D(Vector3D one, Vector3D IIdPoint) {
            double distance = Math.Sqrt(Math.Pow(one.getX() - IIdPoint.getX(), 2) + Math.Pow(one.getZ() - IIdPoint.getZ(), 2));
            return distance;
        }

        public static bool equals(Vector3D first, Vector3D second) {
            return first.x == second.x && first.y == second.y && first.z == second.z;
        }

        public static BlockFace rbf(Bot client, Vector3D target) {
			BlockFace bf;
		
			List<Vector3D> blockfaces = new List<Vector3D>() {
					{ target.add(0.5, 0, 0) },
					{ target.add(0, 0, 0.5)},
					{ target.add(-0.5, 0, 0) },
					{ target.add(0, 0, -0.5)},
					{ target.add(0, 0.5, 0)},
					{ target.add(0, -0.5, 0)}
			};

			foreach (Vector3D b in blockfaces) {
				if (client.getWorld().getBlock(b).canClickTrough()) {
					blockfaces.Remove(b);
				}
			}
			if (blockfaces.Count == 0) {
				blockfaces.Add(target.add(0.5, 0, 0));
				blockfaces.Add(target.add(0, 0, 0.5));
				blockfaces.Add(target.add(-0.5, 0, 0));
				blockfaces.Add(target.add(0, 0, -0.5));
				blockfaces.Add(target.add(0, 0.5, 0));
				blockfaces.Add(target.add(0, -0.5, 0));
			}
		
			Vector3D temp = getNear(client.getEyeLocation(), blockfaces);

			if (temp == null) return BlockFace.UP;

			if (temp.x > target.x) {//x+
				bf = BlockFace.EAST;
			} else if (temp.x < target.x) {//x-
				bf = BlockFace.WEST;
			} else if (temp.z > target.z) {//z+
				bf = BlockFace.SOUTH;
			} else if (temp.z < target.z) {//z-
				bf = BlockFace.NORTH;
			} else if (temp.y > target.y) {//y+
				bf = BlockFace.UP;
			} else if (temp.y < target.y) {//y-
				bf = BlockFace.DOWN;
			} else {
                bf = BlockFace.UP;
            }
			//BotU.log("target:"+target.toString()+" bfc: "+bf.toString());
			return bf;
		}

        public static Vector3D getNear(Vector3D target, List<Vector3D> allPos) {
            Vector3D minpos = null;
            List<Vector3D> temp = new List<Vector3D>();
            foreach (Vector3D position in allPos) {
                if (!equalsInt(position, target)) {
                    double distance = sqrt(position, target);
                    if (minpos == null) {
                        minpos = position;
                    } else {
                        double distanceminpos = sqrt(minpos, target);
                        if (distance < distanceminpos) {
                            minpos = position;
                        } else if (distance == distanceminpos && MathU.rnd(1, 2) == 1) {
                            minpos = position;
                        }
                    }
                }
            }
            temp.Add(minpos);
            foreach (Vector3D position in allPos) {
                if (minpos == null || position == null)
                    return null;
                if (sqrt(minpos, target) == sqrt(position, target)) {
                    temp.Add(position);
                }
            }
            return temp[MathU.rnd(0, temp.Count - 1)];
        }
    }
}
