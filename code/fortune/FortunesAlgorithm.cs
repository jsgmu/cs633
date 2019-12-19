using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    // This implementation of Fortune's algorithm is heavily based on one of the
    // implementations at http://www.raymondhill.net/voronoi/rhill-voronoi.html
    // This works but not for all cases, the broken cases were not tracked down
    // so this is not part of the tool here
    public class FortunesAlgorithm
    {
        private List<Site> Input { get; set; } // call QueueInit to transfer to queue
        private List<QueueItem> Sites { get; set; }
        private List<QueueItem> CircleEvents { get; set; }
        public List<BeachSection> Arcs { get; set; }
        public List<Edge> Edges { get; set; }
        //public List<Cell> Cells { get; set; }
        public Dictionary<int, Cell> Cells { get; set; }

        public FortunesAlgorithm()
        {
            // queues
            Sites = new List<QueueItem>();
            CircleEvents = new List<QueueItem>();

            Arcs = new List<BeachSection>();
            Edges = new List<Edge>();
            Cells = new Dictionary<int, Cell>();
        }

        //    EPSILON: 1e-4,
        //equalWithEpsilon: function(a, b) { return this.abs(a - b) < 1e-4; },
        //greaterThanWithEpsilon: function(a, b) { return (a - b) > 1e-4; },
        //greaterThanOrEqualWithEpsilon: function(a, b) { return (b - a) < 1e-4; },
        //lessThanWithEpsilon: function(a, b) { return (b - a) > 1e-4; },

        private static double epsilon = 1e-4;

        public bool equals(double a, double b) { return Math.Abs(a - b) < epsilon;  }
        public bool gt(double a, double b) { return (a - b) > epsilon; }
        public bool gte(double a, double b) { return (b - a) < epsilon; }
        public bool lt(double a, double b) { return (b - a) > epsilon; }

        public void Reset()
        {
            //this.NUM_SITES_PROCESSED = 0;
            //this.BINARY_SEARCHES = 0;
            //this.BINARY_SEARCH_ITERATIONS = 0;
            //this.PARABOLIC_CUT_CALCS = 0;
            //this.ALL_PARABOLIC_CUT_CALCS = 0;
            //this.BEACHLINE_SIZE = 0;
            //this.CIRCLE_QUEUE_SIZE = 0;
            //this.LARGEST_CIRCLE_QUEUE_SIZE = 0;
            //this.NUM_VOID_EVENTS = 0;
            //this.NUM_CIRCLE_EVENTS = 0;
            //this.TOTAL_NUM_EDGES = 0;
            //this.NUM_DESTROYED_EDGES = 0;
            //this.cellsClosed = false;
            //this.queueInit();
            //this.dumpBeachline();
            //this.draw();

            cellsClosed = false;
            QueueInit();
            // dump beach line
            // draw
        }

        private double sweep = 0;
        private bool cellsClosed = false;

        public void QueueInit()
        {
            //this.sweep = 0;
            //this.siteEvents = [];
            //var n = this.sites.length;
            //for (var i = 0; i < n; i++)
            //{
            //    var site = this.sites[i];
            //    this.queuePushSite({ type: this.SITE_EVENT, x: site.x, y: site.y, site: site});
            //}
            //this.NUM_SITES_PROCESSED = this.siteEvents.length;
            //this.circEvents = [];
            //this.arcs = [];
            //this.edges = [];
            //this.cells = {};

            sweep = 0;
            Sites.Clear();

            for(int i = 0; i < Input.Count; i++)
            {
                var site = Input[i];

                QueuePushSite(site);
            }

            CircleEvents.Clear();
            Arcs.Clear();
            Edges.Clear();
            Cells.Clear();
        }

        public void ClearSites()
        {
            //this.sites = [];
            //this.reset();
            //// reset id generators
            //this.Site.prototype.idgenerator = 1;
            //this.Edge.prototype.idgenerator = 1;

            Sites.Clear();
            Reset();
            Site.NextId = 1;
            Edge.NextId = 1;
        }

        public void AddSite(double x, double y)
        {
            //this.sites.push(new this.Site(x, y));
            //this.reset();
            //this.processQueueAll();
        }

        public void GenerateSites(int n, int width, int height)
        {
            //this.randomSites(n);
            //this.reset();
            //this.processQueueAll();

            RandomSites(n, width, height);
            Reset();
            processQueueAll();
        }

        public void RandomSites(int n, int width, int height)
        {
            //var margin = this.canvasMargin;
            //var xo = this.bbox.xl + margin;
            //var dx = this.bbox.xr - margin * 2;
            //var yo = this.bbox.yt + margin;
            //var dy = this.bbox.yb - margin * 2;
            //for (var i = 0; i < n; i++)
            //{
            //    this.sites.push(new this.Site(this.round(xo + this.random() * dx), this.round(yo + this.random() * dy)));
            //}

            var r = new Random();

            Input = new List<Site>();

            for (int i = 0; i < n; i++)
            {
                var s = new Site(r.Next(0, width), r.Next(0, height));
                //var item = new QueueItem(s);

                //Sites.Add(item);

                Console.WriteLine("INPUT POINT #" + i);
                Console.WriteLine("X=" + s.X + ", Y=" + s.Y);

                Input.Add(s);
            }
        }

        public void QueueSanitize()
        {
            //var q = this.circEvents;
            //var iRight = q.length;
            //if (!iRight) { return; }

            var q = CircleEvents;
            var iRight = q.Count;

            if (iRight == 0)
                return;

            //// remove trailing void events only
            //var iLeft = iRight;
            //while (iLeft && q[iLeft - 1].type === this.VOID_EVENT) { iLeft--; }
            //var nEvents = iRight - iLeft;
            //if (nEvents)
            //{
            //    this.NUM_VOID_EVENTS += nEvents;
            //    q.splice(iLeft, nEvents);
            //}

            var iLeft = iRight;
            while (iLeft != 0 && q[iLeft - 1].IsVoided)
                iLeft--;
            var nEvents = iRight - iLeft;
            if(nEvents > 0)
            {
                q.RemoveRange(iLeft, nEvents);
            }

            //// remove all void events if queue grew too large
            //var nArcs = this.arcs.length;
            //if (q.length < nArcs * 2) { return; }

            var nArcs = Arcs.Count;
            if (q.Count < nArcs * 2)
                return;

            //while (true)
            //{
            //    iRight = iLeft - 1;
            //    // find a right-most void event
            //    while (iRight > 0 && q[iRight - 1].type !== this.VOID_EVENT) { iRight--; }
            //    if (iRight <= 0) { break; }
            //    // find a right-most non-void event immediately to the left of iRight
            //    iLeft = iRight - 1;
            //    while (iLeft > 0 && q[iLeft - 1].type === this.VOID_EVENT) { iLeft--; }
            //    nEvents = iRight - iLeft;
            //    this.NUM_VOID_EVENTS += nEvents;
            //    q.splice(iLeft, nEvents);
            //    // abort if queue has gotten small enough, this allow
            //    // to avoid having to go through the whole array, most
            //    // circle events are added toward the end of the queue
            //    if (q.length < nArcs) { return; }
            //}

            while(true)
            {
                iRight = iLeft - 1;

                while (iRight > 0 && !q[iRight - 1].IsVoided)
                    iRight--;

                if (iRight <= 0)
                    break;

                iLeft = iRight - 1;
                while (iLeft > 0 && q[iLeft - 1].IsVoided)
                    iLeft--;

                nEvents = iRight - iLeft;

                q.RemoveRange(iLeft, nEvents);

                if (q.Count < nArcs)
                    break;
            }
        }

        public bool IsQueueEmpty()
        {
            QueueSanitize();

            return Sites.Count == 0 && CircleEvents.Count == 0;
        }

        public QueueItem QueuePeek()
        {
            //this.queueSanitize();
            //// we will return a site or circle event
            //var siteEvent = this.siteEvents.length > 0 ? this.siteEvents[this.siteEvents.length - 1] : null;
            //var circEvent = this.circEvents.length > 0 ? this.circEvents[this.circEvents.length - 1] : null;
            //// if one and only one is null, the other is a valid event
            //if (Boolean(siteEvent) !== Boolean(circEvent))
            //{
            //    return siteEvent ? siteEvent : circEvent;
            //}
            //// both queues are empty
            //if (!siteEvent)
            //{
            //    return null;
            //}
            //// both queues have valid events, return 'earliest'
            //if (siteEvent.y < circEvent.y || (siteEvent.y == circEvent.y && siteEvent.x < circEvent.x))
            //{
            //    return siteEvent;
            //}
            //return circEvent;

            QueueSanitize();
            QueueItem siteItem, circleItem;

            siteItem = Sites.Count > 0 ? Sites[Sites.Count - 1] : null;
            circleItem = CircleEvents.Count > 0 ? CircleEvents[CircleEvents.Count - 1] : null;

            if (siteItem != null && circleItem == null)
                return siteItem;

            if (siteItem == null && circleItem != null)
                return circleItem;

            if (siteItem == null && circleItem == null)
                return null;

            if (siteItem.Y < circleItem.Y || (siteItem.Y == circleItem.Y && siteItem.X < circleItem.X))
                return siteItem;

            return circleItem;
        }

        public QueueItem QueuePop()
        {
            //    var event = this.queuePeek();
            //if (event) {
            //    if (event.type === this.SITE_EVENT) {
            //    this.siteEvents.pop();
            //}
            //    else {
            //    this.circEvents.pop();
            //}
            //}
            //return event;

            QueueItem item = QueuePeek();

            if(item != null)
            {
                if(item.IsSite())
                {
                    //Sites.RemoveAt(0);
                    Sites.RemoveAt(Sites.Count - 1);
                }
                else
                {
                    //CircleEvents.RemoveAt(0);
                    CircleEvents.RemoveAt(CircleEvents.Count - 1);
                }
            }

            return item;
        }

        public void QueuePushSite(Site s)
        {
            //var q = this.siteEvents;
            //var r = q.length;
            //if (r)
            //{
            //    var l = 0;
            //    var i, c;
            //    while (l < r)
            //    {
            //        i = (l + r) >> 1;
            //        c = o.y - q[i].y;
            //        if (!c) { c = o.x - q[i].x; }
            //        if (c > 0) { r = i; }
            //        else if (c < 0) { l = i + 1; }
            //        else { return; /*Duplicate sites not allowed, quietly ignored*/ }
            //    }
            //    q.splice(l, 0, o);
            //}
            //else
            //{
            //    q.push(o);
            //}

            var q = Sites;
            var r = q.Count;

            if(r > 0)
            {
                var l = 0;
                int i;
                double c;

                while(l < r)
                {
                    i = (l + r) / 2;
                    c = s.Y - q[i].Y;
                    if(c == 0)
                    {
                        c = s.X - q[i].X;
                    }

                    if(c > 0)
                    {
                        r = i;
                    } else if(c < 0)
                    {
                        l = i + 1;
                    } else
                    {
                        return;
                    }
                }

                q.Insert(l, new QueueItem(s));
            } else
            {
                q.Add(new QueueItem(s));
            }
        }

        public QueueItem QueuePushCircle(CircleEvent item)
        {
            var o = new QueueItem(item);

            //var q = this.circEvents;
            //var r = q.length;
            //if (r)
            //{
            //    var l = 0;
            //    var i, c;
            //    while (l < r)
            //    {
            //        i = (l + r) >> 1;
            //        c = o.y - q[i].y;
            //        if (!c) { c = o.x - q[i].x; }
            //        if (c > 0) { r = i; }
            //        else { l = i + 1; }
            //    }
            //    q.splice(l, 0, o);
            //}
            //else
            //{
            //    q.push(o);
            //}

            var q = CircleEvents;
            var r = q.Count;

            if(r > 0)
            {
                int l = 0;
                int i;
                double c;

                while(l < r)
                {
                    i = (l + r) / 2;
                    c = o.Y - q[i].Y;
                    if(c == 0)
                    {
                        c = o.X - q[i].X;
                    }
                    if(c > 0)
                    {
                        r = i;
                    } else
                    {
                        l = i + 1;
                    }
                }

                q.Insert(l, o);
            } else
            {
                q.Add(o);
            }

            return o;
        }

        //public void QueuePush(QueueItem item)
        //{

        //}

        public void processQueueOne()
        {
            //    var event = this.queuePop();
            //if (!event) { return; }
            //this.sweep = event.y;
            //if ( event.type === this.SITE_EVENT ) {
            //    //this.assert(this.cells[event.site.id] === undefined);
            //    this.cells[event.site.id] = new this.Cell(event.site);
            //    // add beach section
            //    this.addArc(event.site);
            //    this.BEACHLINE_SIZE += this.arcs.length;
            //    this.CIRCLE_QUEUE_SIZE += this.circEvents.length;
            //    this.LARGEST_CIRCLE_QUEUE_SIZE = this.max(this.circEvents.length, this.LARGEST_CIRCLE_QUEUE_SIZE);
            //}
            //else {
            //    //this.assert(event.type === this.CIRCLE_EVENT);
            //    // remove beach section
            //    this.removeArc(event);
            //}
            //// wrap-up: close all cells
            //if (this.queueIsEmpty()) {
            //    this.closeCells();
            //}

            var e = QueuePop();

            if (e == null)
                return;

            sweep = e.Y;

            if(e.IsSite())
            {
                //Cells[e.Site.Id] = new Cell(e.Site); // <-- check index
                Cells.Add(e.Site.Id, new Cell(e.Site));
                AddArc(e.Site);
            } else
            {
                RemoveArc(e.CircleEvent);
            }

            // !!!
            //if(IsQueueEmpty())
            //    CloseCells();
        }

        public void processQueueMultiple(int n)
        {
            for(int i = 0; i < n && !IsQueueEmpty(); i++)
            {
                processQueueOne();
            }

            if(IsQueueEmpty())
            {
                // sweep = max of cnnvas height and sweep, in case canvas must expand
            }
        }

        public void processQueueAll()
        {
            while(Sites.Count > 0 || CircleEvents.Count > 0)
            {
                processQueueOne();
            }

            // dump beach line
            // draw
        }

        public Bisector GetBisector(Vertex va, Vertex vb)
        {
            //        var r = { x:(va.x + vb.x) / 2,y: (va.y + vb.y) / 2};
            //    if (vb.y==va.y) {return r;}
            //r.m = (va.x-vb.x)/(vb.y-va.y);
            //    r.b = r.y-r.m* r.x;
            //    return r;

            var b = new Bisector { X = (va.X / vb.X) / 2, Y = (va.Y + vb.Y) / 2 };

            if (vb.Y == va.Y)
                return b;

            b.Slope = (va.X - vb.X) / (vb.Y - va.Y);
            b.SlopeValid = true;
            b.YIntercept = b.Y - b.Slope * b.X;

            return b;
        }

        public double GetLeftBreakPoint(int arcIndex, double sw)
        {
            //var arc = this.arcs[iarc];
            //var site = arc.site;
            //if (site.y == sweep) { return site.x; }
            //if (iarc === 0) { return -Infinity; }
            //return arc.leftParabolicCut(this.arcs[iarc - 1].site, sweep);

            var arc = Arcs[arcIndex];
            var site = arc.Site;
            if (site.Y == sw) { return site.X; }
            if (arcIndex == 0) { return Double.MinValue; }
            return arc.LeftParabolicCut(Arcs[arcIndex - 1].Site, sw);
        }

        public double GetRightBreakPoint(int arcIndex, double sw)
        {
            //if (iarc < this.arcs.length - 1)
            //{
            //    return this.leftBreakPoint(iarc + 1, sweep);
            //}
            //var site = this.arcs[iarc].site;
            //return site.y == sweep ? site.x : Infinity;

            if (arcIndex < Arcs.Count - 1)
            {
                return GetLeftBreakPoint(arcIndex + 1, sw);
            }

            var site = Arcs[arcIndex].Site;

            return site.Y == sw ? site.X : Double.MaxValue;
        }

        public int FindInsertionPoint(double x, double sw)
        {
            //this.BINARY_SEARCHES++;
            //var n = this.arcs.length;
            //if (!n) { return 0; }
            //var l = 0;
            //var r = n;
            //var i;
            //while (l < r)
            //{
            //    this.BINARY_SEARCH_ITERATIONS++;
            //    i = (l + r) >> 1;
            //    if (this.lessThanWithEpsilon(x, this.leftBreakPoint(i, sweep)))
            //    {
            //        r = i;
            //        continue;
            //    }
            //    // check if x after right break point
            //    if (this.greaterThanOrEqualWithEpsilon(x, this.rightBreakPoint(i, sweep)))
            //    {
            //        l = i + 1;
            //        continue;
            //    }
            //    return i;
            //}
            //return l;

            var n = Arcs.Count;

            if (n == 0)
                return 0;

            int l = 0;
            int r = n;
            int i;

            while(l < r)
            {
                i = (l + r) / 2;

                if(lt(x, GetLeftBreakPoint(i, sw)))
                {
                    r = i;
                    continue;
                }

                if(gte(x, GetRightBreakPoint(i, sw)))
                {
                    l = i + 1;
                    continue;
                }

                return i;
            }

            return l;
        }

        public int FindDeletionPoint(double x, double sw)
        {
            //this.BINARY_SEARCHES++;
            //var n = this.arcs.length;
            //if (!n) { return 0; }
            //var l = 0;
            //var r = n;
            //var i;
            //var xcut;
            //while (l < r)
            //{
            //}
            var n = Arcs.Count;
            if (n == 0)
                return 0;

            int l = 0;
            int r = n;
            int i;
            double xcut;

            while(l < r)
            {
                //    this.BINARY_SEARCH_ITERATIONS++;
                //    i = (l + r) >> 1;
                //    xcut = this.leftBreakPoint(i, sweep);
                //    if (this.lessThanWithEpsilon(x, xcut))
                //    {
                //        r = i;
                //        continue;
                //    }
                //    if (this.greaterThanWithEpsilon(x, xcut))
                //    {
                //        l = i + 1;
                //        continue;
                //    }
                //    xcut = this.rightBreakPoint(i, sweep);
                //    if (this.greaterThanWithEpsilon(x, xcut))
                //    {
                //        l = i + 1;
                //        continue;
                //    }
                //    if (this.lessThanWithEpsilon(x, xcut))
                //    {
                //        r = i;
                //        continue;
                //    }
                //    return i;
                i = (l + r) / 2;
                xcut = GetLeftBreakPoint(i, sw);
                if(lt(x, xcut))
                {
                    r = i;
                    continue;
                }
                if(gt(x, xcut))
                {
                    l = i + 1;
                    continue;
                }

                xcut = GetRightBreakPoint(i, sw);
                if(gt(x, xcut))
                {
                    l = i + 1;
                    continue;
                }
                if(lt(x, xcut))
                {
                    r = i;
                    continue;
                }
                return i;
            }

            return l;
            //return 0; // shouldn't happen
        }

        public Edge CreateEdge(Site leftSite, Site rightSite, Vertex va, Vertex vb)
        {
            //var edge = new this.Edge(lSite, rSite);
            //this.edges.push(edge);
            ////this.assert(this.cells[lSite.id] != undefined);
            ////this.assert(this.cells[rSite.id] != undefined);
            //if (va !== undefined)
            //{
            //    this.setEdgeStartpoint(edge, lSite, rSite, va);
            //}
            //if (vb !== undefined)
            //{
            //    this.setEdgeEndpoint(edge, lSite, rSite, vb);
            //}
            //this.cells[lSite.id].halfedges.push(new this.Halfedge(lSite, edge));
            //this.cells[rSite.id].halfedges.push(new this.Halfedge(rSite, edge));
            //return edge;

            var edge = new Edge { LeftSite = leftSite, RightSite = rightSite };

            Edges.Add(edge);

            if(va != null)
            {
                edge.SetStartPoint(leftSite, rightSite, va);
            }

            if(vb != null)
            {
                edge.SetEndPoint(leftSite, rightSite, vb);
            }

            GetCell(leftSite.Id, leftSite).HalfEdges.Add(new HalfEdge { Site = leftSite, Edge = edge });
            GetCell(rightSite.Id, rightSite).HalfEdges.Add(new HalfEdge { Site = rightSite, Edge = edge });

            return edge;
        }

        private Cell GetCell(int index, Site site)
        {
            if (!Cells.ContainsKey(index))
                Cells.Add(index, new Cell(site));

            return Cells[index];
        }

        public Edge CreateBorderEdge(Site site, Vertex va, Vertex vb)
        {
            //createBorderEdge: function(lSite, va, vb) {
            //    var edge = new this.Edge(lSite, null);
            //    edge.va = va;
            //    edge.vb = vb;
            //    this.edges.push(edge);
            //    return edge;
            //},
            var edge = new Edge { LeftSite = site, VA = va, VB = vb };
            Edges.Add(edge);

            return edge;
        }

        public void DestroyEdge(Edge e)
        {
            //destroyEdge: function(edge) {
            //    edge.id = edge.va = edge.vb = undefined;
            //},

            e.Id = -1;
            e.VA = null;
            e.VB = null;
        }

        public Circle CalculateCircle(Site a, Site b, Site c)
        {
            //var ax = a.x;
            //var ay = a.y;
            //var bx = b.x - ax;
            //var by = b.y - ay;
            //var cx = c.x - ax;
            //var cy = c.y - ay;
            //var d = 2 * (bx * cy - by * cx);
            //var hb = bx * bx + by * by;
            //var hc = cx * cx + cy * cy;
            //var x = (cy * hb - by * hc) / d;
            //var y = (bx * hc - cx * hb) / d;
            //return { x: x + ax,y: y + ay,radius: this.sqrt(x * x + y * y)};

            var ax = a.X;
            var ay = a.Y;
            var bx = b.X - ax;
            var by = b.Y - ay;
            var cx = c.X - ax;
            var cy = c.Y - ay;
            var d = 2 * (bx * cy - by * cx);
            var hb = bx * bx + by * by;
            var hc = cx * cx + cy * cy;
            var x = (cy * hb - by * hc) / d;
            var y = (bx * hc - cx * hb) / d;

            return new Circle { X = x + ax, Y = y + ay, Radius = Math.Sqrt(x * x + y * y) };
        }

        public bool VerticesEqual(Vertex va, Vertex vb)
        {
            return equals(va.X, vb.X) && equals(va.Y, vb.Y);
        }

        public void RemoveArc(CircleEvent e)
        {
            //        var x = event.center.x;
            //    var y = event.center.y;
            //    var sweep = event.y;
            //    var deletionPoint = this.findDeletionPoint(x, sweep);
            //    // there could be more than one empty arc at the deletion point, this
            //    // happens when more than two edges are linked by the same vertex,
            //    // so we will collect all those edges by looking up both sides of
            //    // the deletion point

            var x = e.CenterX;
            var y = e.CenterY;
            var sw = e.Y;

            int deletionPoint = FindDeletionPoint(x, sw);

            //    // look left
            //    var iLeft = deletionPoint;
            //    while (iLeft-1 > 0 && this.equalWithEpsilon(x, this.leftBreakPoint(iLeft-1, sweep)) ) {
            //        iLeft--;
            //    }
            int iLeft = deletionPoint;
            while(iLeft - 1 > 0 && equals(x, GetLeftBreakPoint(iLeft-1, sw)))
            {
                iLeft--;
            }

            //// look right
            ///
            //var iRight = deletionPoint;
            //    while (iRight+1 < this.arcs.length && this.equalWithEpsilon(x,this.rightBreakPoint(iRight+1, sweep)) ) {
            //    iRight++;
            //}
            int iRight = deletionPoint;
            while(iRight + 1 < Arcs.Count && equals(x, GetRightBreakPoint(iRight+1, sw)))
            {
                iRight++;
            }

            //// walk through all the collapsed beach sections and set the start point
            //// of their left edge
            //var lArc, rArc;
            //    for (var iArc=iLeft; iArc<=iRight+1; iArc++) {
            //    lArc = this.arcs[iArc - 1];
            //    rArc = this.arcs[iArc];
            //    this.setEdgeStartpoint(rArc.edge, lArc.site, rArc.site, new this.Vertex(x, y));
            //}
            BeachSection lArc, rArc;
            for(int iArc = iLeft; iArc <= iRight + 1; iArc++)
            {
                lArc = Arcs[iArc - 1];
                rArc = Arcs[iArc];

                rArc.Edge.SetStartPoint(lArc.Site, rArc.Site, new Vertex(x, y));
            }


            //    // void circle events of collapsed beach sections and adjacent beach sections
            //    this.voidCircleEvents(iLeft-1,iRight+1);
            VoidCircleEvents(iLeft - 1, iRight + 1);

            //    // removed collapsed beach sections from beachline
            //    this.arcs.splice(iLeft,iRight-iLeft+1);
            Arcs.RemoveRange(iLeft, iRight - iLeft + 1);

            //// create new edge as we have a new transition between
            //// two beach sections which were previously not adjacent
            //lArc = this.arcs [iLeft-1];
            //rArc = this.arcs [iLeft];
            lArc = Arcs[iLeft - 1];
            rArc = Arcs[iLeft];

            //rArc.edge = this.createEdge(lArc.site,rArc.site,undefined,new this.Vertex(x, y));
            rArc.Edge = CreateEdge(lArc.Site, rArc.Site, null, new Vertex(x, y));

            //    // create circle events if any for beach sections left in the beachline
            //    // adjacent to collapsed sections
            //    this.addCircleEvents(iLeft-1,sweep);
            //    this.addCircleEvents(iLeft,sweep);

            AddCircleEvents(iLeft - 1, sw);
            AddCircleEvents(iLeft, sw);
        }

        public void AddArc(Site site)
        {
            // start

            //// find insertion point of new beach section on the beachline
            //var newArc = new this.Beachsection(site);
            //var insertionPoint = this.findInsertionPoint(site.x, site.y);
            BeachSection newArc = new BeachSection { Site = site };
            int insertionPoint = FindInsertionPoint(site.X, site.Y);

            //// case: insert as last beach section, this case can happen only
            //// when *all* previously processed sites have exactly the same
            //// y coordinate.
            //// this case can't result in collapsing beach sections, thus
            //// no circle events need to be generated.
            //if (insertionPoint == this.arcs.length)
            //{

            //    // add new beach section
            //    this.arcs.push(newArc);

            //    // case: first beach section ever means no transitions, means
            //    // no edge is created
            //    if (insertionPoint === 0) { return; }

            //    // case: a new transition between two beach sections is
            //    // created, create an edge for these two beach sections
            //    newArc.edge = this.createEdge(this.arcs[insertionPoint - 1].site, newArc.site);

            //    return;
            //}

            if(insertionPoint == Arcs.Count)
            {
                Arcs.Add(newArc);

                if (insertionPoint == 0)
                    return;

                newArc.Edge = CreateEdge(Arcs[insertionPoint - 1].Site, newArc.Site, null, null);

                return;
            }

            //var lArc, rArc;
            BeachSection lArc, rArc;

            //// case: new beach section to insert falls exactly
            //// in between two existing beach sections:
            //// the net result is that the transition between two existing beach
            //// sections is destroyed -- aka a new end point for one edge is
            //// defined, and two new transitions are created -- aka two new edges
            //// are defined.
            //if (insertionPoint > 0 &&
            //    this.equalWithEpsilon(site.x, this.rightBreakPoint(insertionPoint - 1, site.y)) &&
            //    this.equalWithEpsilon(site.x, this.leftBreakPoint(insertionPoint, site.y)))
            //{

            if(insertionPoint > 0 && equals(site.X, GetRightBreakPoint(insertionPoint - 1, site.Y)) &&
                    equals(site.X, GetLeftBreakPoint(insertionPoint, site.Y)))
            {

                //    // before adding dddd:
                //    //   arcs: aaaaaaaa bbbbbbbb cccccccc
                //    //  edges:          ab       bc
                //    //                  ^
                //    // after adding dddd:
                //    //   arcs: aaaaaaaa dddd bbbbbbbb cccccccc
                //    //  edges:          ad   bd       bc
                //    //                  ^
                //    // transition ab disappears, meaning a new vertex is defined,
                //    // while transition ad and bd appear, meaning two new edges are
                //    // defined
                //    lArc = this.arcs[insertionPoint - 1];
                //    rArc = this.arcs[insertionPoint];
                lArc = Arcs[insertionPoint - 1];
                rArc = Arcs[insertionPoint];

                //    // invalidate circle events of left and right sites
                //    this.voidCircleEvents(insertionPoint - 1, insertionPoint);

                VoidCircleEvents(insertionPoint - 1, insertionPoint);

                //    // an existing transition disappears, meaning a vertex is defined at the
                //    // disappearance point
                //    var circle = this.circumcircle(lArc.site, site, rArc.site);
                //    this.setEdgeStartpoint(rArc.edge, lArc.site, rArc.site, new this.Vertex(circle.x, circle.y));

                var circle = CalculateCircle(lArc.Site, site, rArc.Site);
                rArc.Edge.SetStartPoint(lArc.Site, rArc.Site, new Vertex(circle.X, circle.Y));

                //    // two new transitions appear at the new vertex location
                //    newArc.edge = this.createEdge(lArc.site, newArc.site, undefined, new this.Vertex(circle.x, circle.y));
                //    rArc.edge = this.createEdge(newArc.site, rArc.site, undefined, new this.Vertex(circle.x, circle.y));

                newArc.Edge = CreateEdge(lArc.Site, newArc.Site, null, new Vertex(circle.X, circle.Y));
                rArc.Edge = CreateEdge(newArc.Site, rArc.Site, null, new Vertex(circle.X, circle.Y));

                //    // insert new beach section
                //    this.arcs.splice(insertionPoint, 0, newArc);
                Arcs.Insert(insertionPoint, newArc);

                //    // check whether the left and right beach sections are collapsing
                //    // and if so create circle events, to handle the point of collapse.
                //    this.addCircleEvents(insertionPoint - 1, site.y);
                //    this.addCircleEvents(insertionPoint + 1, site.y);

                AddCircleEvents(insertionPoint - 1, site.Y);
                AddCircleEvents(insertionPoint + 1, site.Y);

                //    return;
                //}
                return;
            }

            //// case: this is the most-likely case, where an existing beach section
            //// is split by the new beach section to insert.
            //// adding a new beach section in the middle of an existing one causes two new
            //// transitions to appear -- but since both transitions involve the same two
            //// sites, only one single edge is created, and assigned to two beach front
            //// transitions (the 'edge' member of the beach section.)

            //// invalidate circle event possibly associated with the beach section
            //// to split
            //this.voidCircleEvents(insertionPoint);
            VoidCircleEvents(insertionPoint);

            //// before:
            ////   arcs: aaaaaaaa bbbbbbbb cccccccc
            ////  edges:          ab       bc
            //// after:
            ////   arcs: aaaaaaaa bbbb dddd bbbb cccccccc
            ////  edges:          ab   bd   db   bc
            ////                        ^   ^
            //// bd & db are actually the same edge, the orientation has just
            //// not been decided yet

            //// insert new beach section into beachline
            //lArc = this.arcs[insertionPoint];
            //rArc = new this.Beachsection(lArc.site);
            //this.arcs.splice(insertionPoint + 1, 0, newArc, rArc);

            lArc = Arcs[insertionPoint];
            rArc = new BeachSection { Site = lArc.Site };
            Arcs.Insert(insertionPoint + 1, newArc);
            Arcs.Insert(insertionPoint + 2, rArc);

            //// since we have a new transition between two beach sections,
            //// a new edge is born
            //newArc.edge = rArc.edge = this.createEdge(lArc.site, newArc.site);
            newArc.Edge = CreateEdge(lArc.Site, newArc.Site, null, null);
            rArc.Edge = newArc.Edge;

            //// check whether the left and right beach sections are collapsing
            //// and if so create circle events, to handle the point of collapse.
            //this.addCircleEvents(insertionPoint, site.y);
            //this.addCircleEvents(insertionPoint + 2, site.y);

            AddCircleEvents(insertionPoint, site.Y);
            AddCircleEvents(insertionPoint + 2, site.Y);

            // end
        }

        public void AddCircleEvents(int iArc, double sw)
        {
            //    addCircleEvents: function(iArc, sweep) {

            //        if (iArc <= 0 || iArc >= this.arcs.length - 1) { return; }
            if (iArc <= 0 || iArc >= Arcs.Count - 1)
                return;

            //        var arc = this.arcs[iArc];
            //        var lSite = this.arcs[iArc - 1].site;
            //        var cSite = this.arcs[iArc].site;
            //        var rSite = this.arcs[iArc + 1].site;
            var arc = Arcs[iArc];
            var lSite = Arcs[iArc - 1].Site;
            var cSite = Arcs[iArc].Site;
            var rSite = Arcs[iArc + 1].Site;

            //        // if any two sites are repeated in the same beach section triplet,
            //        // there can't be convergence
            //        if (lSite.id == rSite.id || lSite.id == cSite.id || cSite.id == rSite.id) { return; }
            if (lSite.Id == rSite.Id || lSite.Id == cSite.Id || cSite.Id == rSite.Id)
                return;

            //        // if points l->c->r are clockwise, then center beach section does not
            //        // converge, hence it can't end up as a vertex
            //        if ((lSite.y - cSite.y) * (rSite.x - cSite.x) <= (lSite.x - cSite.x) * (rSite.y - cSite.y)) { return; }

            if ((lSite.Y - cSite.Y) * (rSite.X - cSite.X) <= (lSite.X - cSite.X) * (rSite.Y - cSite.Y))
            {
                return;
            }

            //        // find circumscribed circle
            //        var circle = this.circumcircle(lSite, cSite, rSite);

            var circle = CalculateCircle(lSite, cSite, rSite);

            //        // not valid if the bottom-most point of the circumcircle
            //        // is above the sweep line
            //        // TODO: And what if it is on the sweep line, should it be discarded if it is
            //        // *before* the last processed x value? Need to think about this.

            //        var ybottom = circle.y + circle.radius;

            var ybottom = circle.Y + circle.Radius;

            //        if (!this.greaterThanOrEqualWithEpsilon(ybottom, sweep)) { return; }
            if (!gte(ybottom, sw))
            {
                return;
            }

            //        var circEvent ={
            //    type: this.CIRCLE_EVENT,
            //    site: cSite,
            //    x: circle.x,
            //    y: ybottom,
            //    center: { x: circle.x, y: circle.y}
            //    };
            //    arc.circleEvent = circEvent;
            //    this.queuePushCircle(circEvent);
            //},
            var ce = new CircleEvent { Site = cSite, X = circle.X, Y = ybottom, CenterX = circle.X, CenterY = circle.Y };
            arc.CircleEvent = QueuePushCircle(ce);
        }

        public void VoidCircleEvents(int left, int right)
        {
            //if (iRight === undefined) { iRight = iLeft; }
            //iLeft = this.max(iLeft, 0);
            //iRight = this.min(iRight, this.arcs.length - 1);
            //while (iLeft <= iRight)
            //{
            //    var arc = this.arcs[iLeft];
            //    if (arc.circleEvent !== undefined)
            //    {
            //        arc.circleEvent.type = this.VOID_EVENT;
            //        // after profiling in Chromium, found out assigning 'undefined' is much more efficient than
            //        // using 'delete' on the property, possibly because 'delete' causes a 're-classify' to trigger
            //        arc.circleEvent = undefined;
            //    }
            //    iLeft++;
            //}

            int iLeft = Math.Max(left, 0);
            int iRight = Math.Min(right, Arcs.Count - 1);

            while(iLeft <= iRight)
            {
                var arc = Arcs[iLeft];

                if(arc.CircleEvent != null)
                {
                    arc.CircleEvent.Void();
                }

                iLeft++;
            }
        }

        public void VoidCircleEvents(int left)
        {
            //if (iRight === undefined) { iRight = iLeft; }
            //iLeft = this.max(iLeft, 0);
            //iRight = this.min(iRight, this.arcs.length - 1);
            //while (iLeft <= iRight)
            //{
            //    var arc = this.arcs[iLeft];
            //    if (arc.circleEvent !== undefined)
            //    {
            //        arc.circleEvent.type = this.VOID_EVENT;
            //        // after profiling in Chromium, found out assigning 'undefined' is much more efficient than
            //        // using 'delete' on the property, possibly because 'delete' causes a 're-classify' to trigger
            //        arc.circleEvent = undefined;
            //    }
            //    iLeft++;
            //}

            int iLeft = Math.Max(left, 0);
            var arc = Arcs[iLeft];

            if (arc.CircleEvent != null)
            {
                arc.CircleEvent.Void();
            }
        }
    }
}
