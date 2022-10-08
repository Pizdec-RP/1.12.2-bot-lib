using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolyBot.Razebator.math;

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
    }
}
