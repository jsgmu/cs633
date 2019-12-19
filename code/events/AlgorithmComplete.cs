using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.events
{
    public class AlgorithmComplete
    {
        public AlgorithmBase AssociatedAlgorithm { get; set; }
        public string Comments { get; set; }
    }
}
