using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class HighlightPointsCommand : AlgorithmStateCommand
    {
        public List<Vector> Points { get; set; }
        public int HighlightLevel { get; set; }
        public bool NonIndexPoints { get; set; }

        public HighlightPointsCommand()
        {
            Points = new List<Vector>();
        }

        public override void applyToCanvas(CanvasWrapper c)
        {
            base.applyToCanvas(c);

            //c.DrawLineUsingActualCoords(P1.X, P1.Y, P2.X, P2.Y);
            c.PushActiveLayer();
            c.SwitchLayer(CanvasWrapper.LAYER_ALGORITHM);
            //c.DrawLineUsingIndexes(StartXIndex, StartYIndex, EndXIndex, EndYIndex);
            //c.DrawLineUsingValues(StartX, StartY, EndX, EndY);
            foreach (var p in Points)
            {
                c.HighlightPointUsingValues(p.X, p.Y, HighlightLevel);
            }
            c.PopActiveLayer();
        }

    }
}
