using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.delaunay
{
    public class DelaunayEdge
    {
        public Vector VStart { get; set; }
        public Vector VEnd { get; set; }
        public bool BadEdge { get; set; }

        public DelaunayEdge()
        {
            BadEdge = false;
        }

        public DelaunayEdge(Vector v1, Vector v2)
        {
            BadEdge = false;

            VStart = v1;
            VEnd = v2;
        }

        public bool AlmostEquals(DelaunayEdge de)
        {
            return (GeomMath.AlmostEqual(VStart, de.VStart) && GeomMath.AlmostEqual(VEnd, de.VEnd)) ||
                   (GeomMath.AlmostEqual(VStart, de.VEnd) && GeomMath.AlmostEqual(VEnd, de.VStart));
        }
    }
}
