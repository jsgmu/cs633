using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.models
{
    public class DemoModel
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Comments { get; set; }
        public int AlgorithmId { get; set; }
        public int AlgorithmIndex { get; set; }
        public AxisModel AxisConfig { get; set; }
        public List<Vector> Points { get; set; }
        public List<LineModel> Lines { get; set; }
        public List<PolygonModel> Polygons { get; set; }

        public DemoModel()
        {
            Points = new List<Vector>();
            Lines = new List<LineModel>();
            Polygons = new List<PolygonModel>();
            AxisConfig = new AxisModel();
        }
    }
}
