using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompGeomVis.datasources
{
    public class PointsDataSource
    {
        public List<DataGridPoint> Points { get; set; }

        public List<DataGridPoint> GetPoints()
        {
            return Points;
        }

        public PointsDataSource()
        {
            Points = new List<DataGridPoint>();
        }

        public void UpdatePoints(List<Vector> points)
        {
            Points.Clear();
            foreach (var v in points)
            {
                Points.Add(new DataGridPoint { X = v.X, Y = v.Y,
                    Highlight1 = v.HighlightLevel == 1,
                    Highlight2 = v.HighlightLevel == 2,
                    Highlight3 = v.HighlightLevel == 3
                });
            }
        }
    }
}
