using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompGeomVis;

namespace CompGeomVis.rotatingcalipers
{
    public class BoundingBox
    {
        public Vector[] Corners { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public BoundingBox()
        {
            Corners = new Vector[4];
        }
    }
}
