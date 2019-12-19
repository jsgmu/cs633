using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.events
{
    public class VectorProcessingStackUpdated
    {
        public AlgorithmBase AssociatedAlgorithm { get; set; }
        public Stack<Vector> ProcessingStack { get; set; }
        public string Comments { get; set; }
    }
}
