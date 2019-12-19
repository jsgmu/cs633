using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class AddNonIndexedPolygonCommand : AlgorithmStateCommand
    {
        public PolygonModel Polygon { get; set; }
        public int Color { get; set; }

        public override void applyToCanvas(CanvasWrapper c)
        {
            base.applyToCanvas(c);

            //c.DrawLineUsingActualCoords(P1.X, P1.Y, P2.X, P2.Y);
            c.PushActiveLayer();
            c.SwitchLayer(CanvasWrapper.LAYER_ALGORITHM);
            //c.DrawLineUsingIndexes(StartXIndex, StartYIndex, EndXIndex, EndYIndex);
            //c.DrawLineUsingValues(StartX, StartY, EndX, EndY);
            c.DrawPolygonUsingValues(Polygon);
            c.PopActiveLayer();
        }
    }
}
