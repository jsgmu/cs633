using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.models
{
    public class AxisModel
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public int XCount { get; set; }
        public int YCount { get; set; }
        public double XStart { get; set; }
        public double YStart { get; set; }

        public AxisModel()
        {
            // Default axis model
            XMin = -200;
            XMax = 200;
            YMin = -100;
            YMax = 100;
            XCount = 40;
            YCount = 20;
            XStart = 5;
            YStart = 5;
        }
    }
}
