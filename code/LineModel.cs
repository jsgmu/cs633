using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class LineModel
    {
        public Vector StartPoint { get; set; }
        public Vector EndPoint { get; set; }
        public int HighlightLevel { get; set; }

        public LineModel()
        {

        }

        public LineModel(LineModel source)
        {
            StartPoint = new Vector { X = source.StartPoint.X, Y = source.StartPoint.Y, Alternates = source.StartPoint.Alternates };
            EndPoint = new Vector { X = source.EndPoint.X, Y = source.EndPoint.Y, Alternates = source.EndPoint.Alternates };
            HighlightLevel = source.HighlightLevel;
        }
    }
}
