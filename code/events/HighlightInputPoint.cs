using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.events
{
    public class HighlightInputPoint
    {
        public AlgorithmBase AssociatedAlgorithm { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int HighlightLevel { get; set; }
        public string Comments { get; set; }
    }
}
