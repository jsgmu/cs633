using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class HighlightNonIndexedLineCommand : AlgorithmStateCommand
    {
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
        public int Color { get; set; }

        public override void applyToCanvas(CanvasWrapper c)
        {
            base.applyToCanvas(c);

            //c.DrawLineUsingActualCoords(P1.X, P1.Y, P2.X, P2.Y);
            c.PushActiveLayer();
            c.SwitchLayer(CanvasWrapper.LAYER_ALGORITHM);
            //c.DrawLineUsingIndexes(StartXIndex, StartYIndex, EndXIndex, EndYIndex);
            c.HighlightLineUsingValues(StartX, StartY, EndX, EndY);
            c.PopActiveLayer();
        }
    }
}
