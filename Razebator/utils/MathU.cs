using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyBot.Razebator.utils {
    internal class MathU {
        private static int[] MULTIPLY_DE_BRUIJN_BIT_POSITION = new int[] { 0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8, 31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9 };
        private static double FRAC_BIAS = 1.7592186044416E13;
        private static double[] ASIN_TAB = new double[257];
        private static double[] COS_TAB = new double[257];
        private static Random rndm = new Random();
        /*private static float[] SIN = make(new float[65536], arrf-> {
        for (int i = 0; i<((float[]) arrf).length; ++i) {
                    arrf[i] = (float) Math.sin((double) i * 3.141592653589793 * 2.0 / 65536.0);
            }
        });*/

        public static int floorDouble(double n) {
            int i = (int)n;
            return n >= i ? i : i - 1;
        }

        public static int ceilDouble(double n) {
            int i = (int)(n + 1);
            return n >= i ? i : i - 1;
        }

        public static int floorFloat(float n) {
            int i = (int)n;
            return n >= i ? i : i - 1;
        }

        public static int ceilFloat(float n) {
            int i = (int)(n + 1);
            return n >= i ? i : i - 1;
        }

        /*public static double atan2(double d, double d2) {
            bool bl;
            double d3;
            bool bl2;
            bool bl3;
            double d4 = d2 * d2 + d * d;
            if (Double.IsNaN(d4)) {
                return Double.NaN;
            }
            bool bl4 = bl2 = d < 0.0;
            if (bl2) {
                d = -d;
            }
            bool bl5 = bl = d2 < 0.0;
            if (bl) {
                d2 = -d2;
            }
            bool bl6 = bl3 = d > d2;
            if (bl3) {
                d3 = d2;
                d2 = d;
                d = d3;
            }
            d3 = fastInvSqrt(d4);
            double d5 = FRAC_BIAS + (d *= d3);
            int n = (int)Double.doubleToRawLongBits(d5);
            double d6 = ASIN_TAB[n];
            double d7 = COS_TAB[n];
            double d8 = d5 - FRAC_BIAS;
            double d9 = d * d7 - (d2 *= d3) * d8;
            double d10 = (6.0 + d9 * d9) * d9 * 0.16666666666666666;
            double d11 = d6 + d10;
            if (bl3) {
                d11 = 1.5707963267948966 - d11;
            }
            if (bl) {
                d11 = 3.141592653589793 - d11;
            }
            if (bl2) {
                d11 = -d11;
            }
            return d11;
        }*/

        public static int wrapDegrees(int n) {
            int n2 = n % 360;
            if (n2 >= 180) {
                n2 -= 360;
            }
            if (n2 < -180) {
                n2 += 360;
            }
            return n2;
        }

        public static float wrapDegrees(float f) {
            float f2 = f % 360.0f;
            if (f2 >= 180.0f) {
                f2 -= 360.0f;
            }
            if (f2 < -180.0f) {
                f2 += 360.0f;
            }
            return f2;
        }

        public static double wrapDegrees(double d) {
            double d2 = d % 360.0;
            if (d2 >= 180.0) {
                d2 -= 360.0;
            }
            if (d2 < -180.0) {
                d2 += 360.0;
            }
            return d2;
        }

        /*public static float fastInvSqrt(float f) {
            float f2 = 0.5f * f;
            int n = Float.floatToIntBits(f);
            n = 1597463007 - (n >> 1);
            f = Float.intBitsToFloat(n);
            f *= 1.5f - f2 * f * f;
            return f;
        }

        public static double fastInvSqrt(double d) {
            double d2 = 0.5 * d;
            long l = Double.doubleToRawLongBits(d);
            l = 6910469410427058090L - (l >> 1);
            d = Double.longBitsToDouble(l);
            d *= 1.5 - d2 * d * d;
            return d;
        }*/

        public static double getDir(double d) {
            if (d < 0)
                return -1;
            else if (d > 0)
                return 1;
            else
                return 0;
        }

        /*public static < T > T make(T t, Consumer < T > consumer) {
            consumer.accept(t);
            return t;
        }*/

        public static int rnd(double min, double max) {
            max -= min;
            return (int)((rndm.NextSingle() * ++max) + min);
        }

        public static T random<T>(List<T> list) {
            return list[(rnd(0, list.Count - 1))];
        }

        /*public static float sin(float f) {
            return SIN[(int)(f * 10430.378f) & 0xFFFF];
        }

        public static float cos(float f) {
            return SIN[(int)(f * 10430.378f + 16384.0f) & 0xFFFF];
        }*/

        public static double toRadians(double degrees) {
            return (Math.PI / 180) * degrees;
        }

        public static double fround(float f) {
            return (double)f;
        }

        public static double round(double numberToRound, int decimalPlaces) {
            if (Double.IsNaN(numberToRound))
                return Double.NaN;

            double factor = 1;
            for (int i = 0; i < Math.Abs(decimalPlaces); i++)
                if (decimalPlaces > 0)
                    factor *= 10;
                else
                    factor /= 10;

            return (double)Math.Round(numberToRound * factor) / factor;
        }

        public static int clamp(int num, int min, int max) {
            if (num < min) {
                return min;
            } else {
                return num > max ? max : num;
            }
        }

        /**
         * Returns the value of the first parameter, clamped to be within the lower and upper limits given by the second and
         * third parameters
         */
        public static float clamp(float num, float min, float max) {
            if (num < min) {
                return min;
            } else {
                return num > max ? max : num;
            }
        }

        public static double clamp(double num, double min, double max) {
            if (num < min) {
                return min;
            } else {
                return num > max ? max : num;
            }
        }

        public static int ceil(float value) {
            int i = (int)value;
            return value > (float)i ? i + 1 : i;
        }

        public static int ceil(double value) {
            int i = (int)value;
            return value > (double)i ? i + 1 : i;
        }

        public static double Truncate(double value) {
            if (value < 0) {
                return Math.Ceiling(value);
            } else {
                return Math.Floor(value);
            }
        }
    }
}
