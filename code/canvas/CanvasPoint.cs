using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.canvas
{
    // Point handling is awkward, would be better to collapse this class with Vector class
    // and let them serve all purposes, perhaps having an ambient canvas configuration
    // so points can be re-computed on the fly anytime specific coordinates are missing
    public class CanvasPoint
    {
        // Coordinate forms:
        // - index of dot in grid (integers)
        // - (x,y) of one of these dots, dependent on axis ranges (doubles)
        // - (x,y) of location in canvas of one of these dots (doubles)
        public bool HasCorrespondingDot { get; set; }
        public int DotIndexLeft { get; set; }
        public int DotIndexTop { get; set; }
        public double DotLeft { get; set; }
        public double DotTop { get; set; }
        public double DotLeftPostOffset { get; set; }
        public double DotTopPostOffset { get; set; }
        public double ValueX { get; set; }
        public double ValueY { get; set; }
        public CanvasConfiguration Config { get; set; }

        public CanvasPoint()
        {
            DotLeftPostOffset = 0;
            DotTopPostOffset = 0;
        }
        public void SetPoint()
        {

        }
    }
}
