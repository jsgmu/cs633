using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class BeachSection
    {
        public Site Site { get; set; }
        public Edge Edge { get; set; }

        public double Sweep { get; set; }
        public int LId { get; set; }
        public QueueItem CircleEvent { get; set; }

        public override string ToString()
        {
            var siteStr = (Site == null) ? "null" : Site.ToString();
            var circleEvent = CircleEvent == null ? null : CircleEvent.CircleEvent;
            var ceStr = circleEvent == null ? "null" : circleEvent.ToString();

            return "(site=" + siteStr + "; circleEvent=" + ceStr + ")";
        }

        public bool IsCollapsing()
        {
            return CircleEvent != null && !CircleEvent.IsVoided;
        }

        public CircleEvent GetCircleEvent()
        {
            return CircleEvent.CircleEvent;
        }

        public static double LeftParabolicCut(Site site, Site left, double dir)
        {
            //var rfocx = site.x;
            //var rfocy = site.y;
            //// parabola in degenerate case where focus is on directrix
            //if (rfocy == directrix) { return rfocx; }
            //var lfocx = left.x;
            //var lfocy = left.y;
            //// parabola in degenerate case where focus is on directrix
            //if (lfocy == directrix) { return lfocx; }
            //// both parabolas have same distance to directrix, thus break point is midway
            //if (rfocy == lfocy) { return (rfocx + lfocx) / 2; }
            //// calculate break point the normal way
            //var pby2 = rfocy - directrix;
            //var plby2 = lfocy - directrix;
            //var hl = lfocx - rfocx;
            //var aby2 = 1 / pby2 - 1 / plby2;
            //var b = hl / plby2;
            //return (-b + this.sqrt(b * b - 2 * aby2 * (hl * hl / (-2 * plby2) - lfocy + plby2 / 2 + rfocy - pby2 / 2))) / aby2 + rfocx;

            //this.PARENT.PARABOLIC_CUT_CALCS++;
            var rfocx = site.X;
            var rfocy = site.Y;
            // parabola in degenerate case where focus is on directrix
            if (rfocy == dir) { return rfocx; }
            var lfocx = left.X;
            var lfocy = left.Y;
            // parabola in degenerate case where focus is on directrix
            if (lfocy == dir) { return lfocx; }
            // both parabolas have same distance to directrix, thus break point is midway
            if (rfocy == lfocy) { return (rfocx + lfocx) / 2; }
            // calculate break point the normal way
            var pby2 = rfocy - dir;
            var plby2 = lfocy - dir;
            var hl = lfocx - rfocx;
            var aby2 = 1 / pby2 - 1 / plby2;
            var b = hl / plby2;

            return (-b + Math.Sqrt(b * b - 2 * aby2 * (hl * hl / (-2 * plby2) - lfocy + plby2 / 2 + rfocy - pby2 / 2))) / aby2 + rfocx;
        }

        public double LeftParabolicCut(Site left, double dir)
        {
            return LeftParabolicCut(Site, left, dir);
        }
    }
}
