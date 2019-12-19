using CompGeomVis.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class AddTextStatusCommand : AlgorithmStateCommand
    {
        public override void apply()
        {
            EventBus.Publish<AddTextStatus>(new AddTextStatus { AssociatedAlgorithm = AssociatedAlgorithm, Text = Comments });
        }
    }
}
