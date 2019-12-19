using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class Edge
    {
        public int Id { get; set; }
        public static int NextId = 1;

        public Site LeftSite { get; set; }
        public Site RightSite { get; set; }
        public Vertex VA { get; set; }
        public Vertex VB { get; set; }

        public Edge()
        {
            Id = NextId;
            NextId++;
        }

        public bool IsLineSegment()
        {
            // if there's a start and end point, it's a line
            return VA != null && VB != null;
        }

        public void SetStartPoint(Site leftSite, Site rightSite, Vertex vertex)
        {
            //if (edge.va === undefined && edge.vb === undefined)
            //{
            //    edge.va = vertex;
            //    edge.lSite = lSite;
            //    edge.rSite = rSite;
            //}
            //else if (edge.lSite.id == rSite.id)
            //{
            //    //this.assert(edge.vb === undefined);
            //    edge.vb = vertex;
            //}
            //else
            //{
            //    //this.assert(edge.va === undefined);
            //    edge.va = vertex;
            //}

            if(VA == null && VB == null)
            {
                VA = vertex;
                LeftSite = leftSite;
                RightSite = rightSite;
            } else if(leftSite.Id == rightSite.Id)
            {
                VB = vertex;
            } else
            {
                VA = vertex;
            }
        }

        public void SetEndPoint(Site leftSite, Site rightSite, Vertex vertex)
        {
            SetStartPoint(rightSite, leftSite, vertex);
        }
    }
}
