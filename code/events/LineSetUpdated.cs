using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.events
{
    public class LineSetUpdated
    {
        public string Label { get; set; }
        public List<LineModel> Lines { get; set; }
    }
}
