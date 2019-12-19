using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompGeomVis.events
{
    public class PointSetUpdated
    {
        public string Label { get; set; }
        public List<Vector> Points { get; set; }
    }
}
