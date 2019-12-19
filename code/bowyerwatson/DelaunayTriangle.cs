using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.delaunay
{
    public class DelaunayTriangle
    {
        public Vector V1 { get; set; }
        public Vector V2 { get; set; }
        public Vector V3 { get; set; }
        public bool IsBad { get; set; }

        public DelaunayTriangle()
        {
            IsBad = false;
        }
        public bool AlmostEqual(DelaunayTriangle t2)
        {
            bool side1 = GeomMath.AlmostEqual(V1, t2.V1) || GeomMath.AlmostEqual(V1, t2.V2) || GeomMath.AlmostEqual(V1, t2.V3);
            bool side2 = GeomMath.AlmostEqual(V2, t2.V1) || GeomMath.AlmostEqual(V2, t2.V2) || GeomMath.AlmostEqual(V2, t2.V3);
            bool side3 = GeomMath.AlmostEqual(V3, t2.V1) || GeomMath.AlmostEqual(V3, t2.V2) || GeomMath.AlmostEqual(V3, t2.V3);

            return side1 && side2 && side3;
        }

        public bool HasVertex(Vector v)
        {
            return GeomMath.AlmostEqual(V1, v) || GeomMath.AlmostEqual(V2, v) || GeomMath.AlmostEqual(V3, v);
        }

        public bool CircumcircleContainsVertex(Vector v)
        {
            double ab = V1.norm2();
            double cd = V2.norm2();
            double ef = V3.norm2();

            double ax = V1.X;
            double ay = V1.Y;

            double bx = V2.X;
            double by = V2.Y;

            double cx = V3.X;
            double cy = V3.Y;

            double circum_x = (ab * (cy - by) + cd * (ay - cy) + ef * (by - ay)) / (ax * (cy - by) + bx * (ay - cy) + cx * (by - ay));
            double circum_y = (ab * (cx - bx) + cd * (ax - cx) + ef * (bx - ax)) / (ay * (cx - bx) + by * (ax - cx) + cy * (bx - ax));

            Vector center = new Vector { X = circum_x * 0.5, Y = circum_y * 0.5 };
            double radius = V1.dist2(center);
            double dist = v.dist2(center);

            return dist <= radius;
        }
    }
}
