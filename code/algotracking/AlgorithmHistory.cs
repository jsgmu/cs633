using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class AlgorithmHistory
    {
        public List<AlgorithmStatusLayer> StatusLayers { get; set; }

        public AlgorithmHistory()
        {
            StatusLayers = new List<AlgorithmStatusLayer>();
        }

        public void AddLayer(AlgorithmStatusLayer layer)
        {
            StatusLayers.Add(layer);
        }

        public AlgorithmStatusLayer CreateAndAddNewLayer(string comments = "")
        {
            var layer = new AlgorithmStatusLayer { Comments = comments };
            StatusLayers.Add(layer);
            return layer;
        }
    }
}
