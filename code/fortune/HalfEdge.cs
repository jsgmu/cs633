using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class HalfEdge : IComparable<HalfEdge>
    {
        public Site Site { get; set; }
        public Edge Edge { get; set; }
        public double Angle { get; set; }

        public HalfEdge()
        {
        }

        public HalfEdge(Edge edge, Site leftSite, Site rightSite)
        {
            Site = leftSite;
            Edge = edge;

            //if (rSite)
            //{
            //    this.angle = Math.atan2(rSite.y - lSite.y, rSite.x - lSite.x);
            //}
            //else
            //{
            //    var va = edge.va,
            //        vb = edge.vb;
            //    // rhill 2011-05-31: used to call getStartpoint()/getEndpoint(),
            //    // but for performance purpose, these are expanded in place here.
            //    this.angle = edge.lSite === lSite ?
            //        Math.atan2(vb.x - va.x, va.y - vb.y) :
            //        Math.atan2(va.x - vb.x, vb.y - va.y);
            //}
            if (rightSite != null)
            {
                Angle = Math.Atan2(rightSite.Y - leftSite.Y, rightSite.X - leftSite.X);
            } else
            {
                var va = edge.VA;
                var vb = edge.VB;

                Angle = edge.LeftSite == leftSite ?
                    Math.Atan2(vb.X - va.X, va.Y - vb.Y) :
                    Math.Atan2(va.X - vb.X, vb.Y - va.Y);
            }
        }

        public Vertex GetStartPoint()
        {
            if (Edge.LeftSite.Id == Site.Id)
                return Edge.VA;

            return Edge.VB;
        }

        public Vertex GetEndPoint()
        {
            if (Edge.LeftSite.Id == Site.Id)
                return Edge.VB;

            return Edge.VA;
        }

        public int CompareTo(HalfEdge other)
        {
            //var ava = a.getStartpoint();
            //var avb = a.getEndpoint();
            //var bva = b.getStartpoint();
            //var bvb = b.getEndpoint();
            //return self.Math.atan2(bvb.y - bva.y, bvb.x - bva.x) - self.Math.atan2(avb.y - ava.y, avb.x - ava.x);

            var ava = GetStartPoint();
            var avb = GetEndPoint();
            var bva = other.GetStartPoint();
            var bvb = other.GetEndPoint();

            return (int)(Math.Atan2(bvb.Y - bva.Y, bvb.X - bva.X) - Math.Atan2(avb.Y - ava.Y, avb.X - ava.X));
        }

    }
}
