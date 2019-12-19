using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.events
{
    public class InputPointSelected
    {
        public int XIndex { get; set; }
        public int YIndex { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public InputPointSelected(int xi, int yi, double xp, double yp)
        {
            XIndex = xi;
            YIndex = yi;
            X = xp;
            Y = yp;
        }
    }
}
