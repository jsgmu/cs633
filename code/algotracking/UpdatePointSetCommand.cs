using CompGeomVis.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class UpdatePointSetCommand : AlgorithmStateCommand
    {
        public string Label { get; set; }
        public List<Vector> Points { get; set; }

        public override void apply()
        {
            EventBus.Publish<PointSetUpdated>(new PointSetUpdated
            {
                Label = Label,
                Points = Points
            });
        }
    }
}
