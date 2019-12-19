using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.events
{
    public class AddTextStatus
    {
        public AlgorithmBase AssociatedAlgorithm { get; set; }
        public string Text { get; set; }
    }
}
