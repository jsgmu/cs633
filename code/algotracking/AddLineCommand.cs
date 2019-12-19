using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompGeomVis.canvas;

namespace CompGeomVis.algotracking
{
    public class AddLineCommand : AlgorithmStateCommand
    {
        //public Vector P1 { get; set; }
        //public Vector P2 { get; set; }
        public int StartXIndex { get; set; }
        public int StartYIndex { get; set; }
        public int EndXIndex { get; set; }
        public int EndYIndex { get; set; }
        public int Color { get; set; }

        public override void applyToCanvas(CanvasWrapper c)
        {
            base.applyToCanvas(c);

            //c.DrawLineUsingActualCoords(P1.X, P1.Y, P2.X, P2.Y);
            c.PushActiveLayer();
            c.SwitchLayer(CanvasWrapper.LAYER_ALGORITHM);
            c.DrawLineUsingIndexes(StartXIndex, StartYIndex, EndXIndex, EndYIndex);
            c.PopActiveLayer();
        }
    }
}
