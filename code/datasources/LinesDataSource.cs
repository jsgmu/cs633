using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.datasources
{
    public class LinesDataSource
    {
        public List<DataGridLine> Lines { get; set; }

        public List<DataGridLine> GetLines()
        {
            return Lines;
        }

        public LinesDataSource()
        {
            Lines = new List<DataGridLine>();
        }

        public void UpdateLines(List<LineModel> lines)
        {
            Lines.Clear();
            foreach (var line in lines)
            {
                Lines.Add(new DataGridLine
                {
                    StartX = line.StartPoint.X,
                    StartY = line.StartPoint.Y,
                    EndX = line.EndPoint.X,
                    EndY = line.EndPoint.Y,
                    Highlight1 = line.HighlightLevel == 1,
                    Highlight2 = line.HighlightLevel == 2,
                    Highlight3 = line.HighlightLevel == 3
                });
            }
        }
    }
}
