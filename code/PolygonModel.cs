using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class PolygonModel
    {
        public List<LineModel> Lines { get; set; }
        public bool IsClosed { get {
                return Lines != null && Lines.Count >= 3 && Lines[0].StartPoint.Equals(Lines[Lines.Count - 1].EndPoint);
        } }
        public string Comments { get; set; }
        public int HighlightLevel { get; set; }
        public Color Color { get; set; }

        public PolygonModel()
        {
            Lines = new List<LineModel>();
        }

        public PolygonModel(PolygonModel source)
        {
            Lines = new List<LineModel>();

            foreach (var line in source.Lines)
            {
                Lines.Add(new LineModel(line));
            }

            Comments = source.Comments;
            HighlightLevel = source.HighlightLevel;
            Color = source.Color;
        }
    }
}
