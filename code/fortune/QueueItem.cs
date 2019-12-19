using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class QueueItem
    {
        public int Type { get; set; } // 0 = site, 1 = circle
        public Site Site { get; set; }
        public CircleEvent CircleEvent { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsVoided { get; set; }

        public QueueItem(Site site)
        {
            Site = site;
            X = Site.X;
            Y = Site.Y;
            Type = 0;
            IsVoided = false;
        }

        public QueueItem(CircleEvent ce)
        {
            CircleEvent = ce;
            X = ce.Site.X;
            Y = ce.Site.Y;
            Type = 1;
            IsVoided = false;
        }

        public bool IsSite()
        {
            return Type == 0;
        }

        public bool IsCircle()
        {
            return Type == 1;
        }

        public void Void()
        {
            IsVoided = true;
        }
    }
}
