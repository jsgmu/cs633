using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class Bisector
    {
        public double Slope { get; set; }
        public double YIntercept { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool SlopeValid { get; set; }

        public Bisector()
        {
            SlopeValid = false;
        }
    }
}
