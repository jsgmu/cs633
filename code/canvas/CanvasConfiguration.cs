using CompGeomVis.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.canvas
{
    public class CanvasConfiguration
    {
        public double Thickness { get; set; }
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
        public int XCount { get; set; }
        public int YCount { get; set; }
        public double XStart { get; set; }
        public double YStart { get; set; }
        
        // Below properties are computed only
        public double XIncrement { get; set; }
        public double YIncrement { get; set; }
        public double ValueTickX { get; set; }
        public double ValueTickY { get; set; }

        public string FormatAxis()
        {
            return "X:[" + XMin + "," + XMax + "], Y:[" + YMin + ", " + YMax + "]";
        }

        public void BuildConfiguration(double xmin, double xmax, double ymin, double ymax,
                                       double canvasWidth, double canvasHeight,
                                       int xCount, int yCount, double xStart, double yStart)
        {
            XMin = xmin;
            YMin = ymin;
            XMax = xmax;
            YMax = ymax;
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            XCount = xCount;
            YCount = yCount;
            XStart = xStart;
            YStart = yStart;

            XIncrement = (CanvasWidth - XStart) / XCount;
            YIncrement = (CanvasHeight - YStart) / YCount;

            ValueTickX = (Math.Abs(xmin) + Math.Abs(xmax)) / xCount;
            ValueTickY = (Math.Abs(ymin) + Math.Abs(ymax)) / yCount;
        }

        public void BuildConfiguration(AxisModel axis, double canvasWidth, double canvasHeight)
        {
            BuildConfiguration(axis.XMin, axis.XMax, axis.YMin, axis.YMax,
                canvasWidth, canvasHeight,
                axis.XCount, axis.YCount, axis.XStart, axis.YStart);
        }

        public void Recompute(double canvasWidth, double canvasHeight)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;

            XIncrement = (CanvasWidth - XStart) / XCount;
            YIncrement = (CanvasHeight - YStart) / YCount;
        }
    }
}
