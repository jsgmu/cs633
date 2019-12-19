using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class Cell
    {
        public Site Site { get; set; }
        public List<HalfEdge> HalfEdges { get; set; }

        public Cell(Site s)
        {
            Site = s;
            HalfEdges = new List<HalfEdge>();
        }

        public void PrepareHalfEdges()
        {
            for(int i = HalfEdges.Count - 1; i >= 0; i--)
            {
                if(HalfEdges[i].GetStartPoint() == null || HalfEdges[i].GetEndPoint() == null)
                {
                    HalfEdges.RemoveAt(i);
                }
            }

            HalfEdges.Sort();
        }
    }
}
