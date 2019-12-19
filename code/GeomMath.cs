using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class GeomMath
    {
        public const int DIRECTION_CLOCKWISE = 1;
        public const int DIRECTION_COUNTERCLOCKWISE = 2;
        public const int DIRECTION_NONE = 3; // collinear

        public static double CrossProduct(Vector v1, Vector v2, Vector v3)
        {
            //           given p[k], p[k + 1], p[k + 2] each with coordinates x, y:
            //dx1 = x[k + 1] - x[k]
            //dy1 = y[k + 1] - y[k]
            //dx2 = x[k + 2] - x[k + 1]
            //dy2 = y[k + 2] - y[k + 1]
            //zcrossproduct = dx1 * dy2 - dy1 * dx2

            double dx1 = v2.X - v1.X;
            double dy1 = v2.Y - v1.Y;
            double dx2 = v3.X - v2.X;
            double dy2 = v3.Y - v2.Y;

            return dx1 * dy2 - dy1 * dx2;
        }

        public static int GetTurnDirection(Vector v1, Vector v2, Vector v3)
        {
            //double crossProduct = (((long)b.x - a.x) * ((long)c.y - a.y)) -
            //        (((long)b.y - a.y) * ((long)c.x - a.x));

            //if (crossProduct > 0)
            //{
            //    return Turn.COUNTER_CLOCKWISE;
            //}
            //else if (crossProduct < 0)
            //{
            //    return Turn.CLOCKWISE;
            //}
            //else
            //{
            //    return Turn.COLLINEAR;
            //}

            double crossProduct = (v2.X - v1.X) * (v3.Y - v1.Y) - (v2.Y - v1.Y) * (v3.X - v1.X);

            if (crossProduct > 0)
                return DIRECTION_COUNTERCLOCKWISE;

            if (crossProduct < 0)
                return DIRECTION_CLOCKWISE;

            return DIRECTION_NONE;
        }

        public static double Distance(Vector v1, Vector v2)
        {
            //    double distanceA = Math.sqrt((((long)lowest.x - a.x) * ((long)lowest.x - a.x)) +
            //            (((long)lowest.y - a.y) * ((long)lowest.y - a.y)));
            //    double distanceB = Math.sqrt((((long)lowest.x - b.x) * ((long)lowest.x - b.x)) +
            //            (((long)lowest.y - b.y) * ((long)lowest.y - b.y)));

            double c1 = v1.X - v2.X;
            double c2 = v2.Y - v2.Y;

            return Math.Sqrt(c1 * c1 + c2 * c2);
        }

        public static bool AlmostEqual(double value1, double value2)
        {
            double difference = Math.Abs(value1 * .00001);

            // Compare the values
            // The output to the console indicates that the two values are equal
            return (Math.Abs(value1 - value2) <= difference);
        }

        public static bool AlmostEqual(Vector v1, Vector v2)
        {
            return AlmostEqual(v1.X, v2.X) && AlmostEqual(v1.Y, v2.Y);
        }

        public static double DotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vector Subtract(Vector v1, Vector v2)
        {
            return new Vector { X = v1.X - v2.X, Y = v1.Y - v2.Y };
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }

        public static Vector Multiply(Vector v1, double value)
        {
            return new Vector { X = v1.X * value, Y = v1.Y * value };
        }
    }
}
