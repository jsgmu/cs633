using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.canvas
{
    public class CoordHelper
    {
        public static CanvasPoint TranslateFromMouse(CanvasConfiguration c, double xp, double yp)
        {
            var p = new CanvasPoint();

            p.Config = c;
            p.DotLeft = xp;
            p.DotTop = yp;
            p.DotIndexLeft = (int)((xp - c.XStart + c.XIncrement / 2) / c.XIncrement);
            p.DotIndexTop = (int)((yp - c.YStart + c.YIncrement / 2) / c.YIncrement);

            p.ValueX = c.ValueTickX * p.DotIndexLeft + c.XMin;
            p.ValueY = c.ValueTickY * (c.YCount - p.DotIndexTop) + c.YMin;

            return p;
        }

        public static CanvasPoint TranslateFromValues(CanvasConfiguration c, double x, double y)
        {
            var p = new CanvasPoint();

            p.ValueX = x;
            p.ValueY = y;

            p.DotIndexLeft = (int)((p.ValueX - c.XMin) / c.ValueTickX);
            p.DotIndexTop = (int)((p.ValueY - c.YMin) / c.ValueTickY);
            p.DotIndexTop = -(p.DotIndexTop - c.YCount);

            return p;
        }
    }
}
