using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class CircleEvent
    {
        public Site Site { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public TreeNode Arc { get; set; }

        public CircleEvent()
        {
        }
    }
}
