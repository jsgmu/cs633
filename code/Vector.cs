using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    // This class is a little awkward, see CanvasPoint, but generally this
    // is the "Point" workhorse class for representing 2D points for processing
    // by algorithms
    public class Vector : IComparable<Vector>
    {
        public string Label { get; set; }
        public bool Is2D { get; set; }
        public bool Is3D { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int SortMode { get; set; }
        public CanvasPoint Alternates { get; set; }
        public int HighlightLevel { get; set; }

        public Vector()
        {
            SortMode = 1;
        }
        //, Alternates = new CanvasPoint { DotIndexLeft = xi1, DotIndexTop = yi1 }
        public Vector(double _x, double _y)
        {
            SortMode = 1;
            X = _x;
            Y = _y;
        }

        public Vector(double _x, double _y, int xi, int yi)
        {
            SortMode = 1;
            X = _x;
            Y = _y;
            Alternates = new CanvasPoint { DotIndexLeft = xi, DotIndexTop = yi, ValueX = _x, ValueY = _y };
        }

        public int CompareTo(Vector other)
        {
            if(SortMode == 1)
            {
                if (Y < other.Y)
                    return -1;

                else if (Y > other.Y)
                {
                    return 1;
                } else if (Y == other.Y)
                {
                    if (X < other.X)
                        return -1;
                    else if (X > other.X)
                        return 1;
                }

                return 0;
            } else
            {
                // if sortmode == 2
                return 0;
            }
        }

        public double dist2(Vector v)
        {
            double dx = X - v.X;
            double dy = Y - v.Y;

            return dx * dx + dy * dy;
        }

        public double norm2()
        {
            return X * X + Y * Y;
        }

        public Vector normalize()
        {
            var mag = norm2();

            return new Vector { X = X / mag, Y = Y / mag };
        }

        public double at(int index)
        {
            if (index == 0)
                return X;

            return Y;
        }

        public override bool Equals(object o)
        {
            if (!(o is Vector))
                return false;

            var v = o as Vector;

            return GeomMath.AlmostEqual(this, v);
        }
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
