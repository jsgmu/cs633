using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class AlgorithmStateCommand
    {
        public AlgorithmBase AssociatedAlgorithm { get; set; }
        public string Comments { get; set; }

        public bool HasComments()
        {
            return !string.IsNullOrEmpty(Comments);
        }

        public virtual void applyToCanvas(CanvasWrapper c)
        {
        }

        public virtual void apply()
        {
        }
    }
}
