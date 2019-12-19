using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class VectorProcessingStackUpdatedCommand : AlgorithmStateCommand
    {
        public Stack<Vector> ProcessingStack { get; set; }
    }
}
