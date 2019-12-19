using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.datasources
{
    public class DataGridLine
    {
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
        public string Comments { get; set; }
        public bool Highlight1 { get; set; }
        public bool Highlight2 { get; set; }
        public bool Highlight3 { get; set; }
    }
}
