using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class HighlightInputPointCommand : AlgorithmStateCommand
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int HightlightLevel { get; set; }
    }
}
