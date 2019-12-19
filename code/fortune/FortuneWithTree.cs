using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class FortuneWithTree
    {
        private List<Site> Input { get; set; } // call QueueInit to transfer to queue
        private List<Site> Sites { get; set; }
        //private List<QueueItem> SiteQueue { get; set; }
        //private RBTree SiteTree { get; set; }
        //private List<QueueItem> CircleEventQueue { get; set; } // circle events - becomes tree
        //private RBTree CircleEventTree { get; set; }
        ////public List<BeachSection> Arcs { get; set; } // beach line - becomes tree
        //private RBTree Arcs { get; set; }
        public List<Edge> Edges { get; set; }
        public Dictionary<int, Cell> Cells { get; set; }
        public List<Vertex> Vertices { get; set; }
        public RBTree BeachLine { get; set; }
        public RBTree CircleEvents { get; set; }

        public FortuneWithTree()
        {
            //this.vertices = null;
            //this.edges = null;
            //this.cells = null;
            //this.toRecycle = null;
            //this.beachsectionJunkyard = [];
            //this.circleEventJunkyard = [];
            //this.vertexJunkyard = [];
            //this.edgeJunkyard = [];
            //this.cellJunkyard = [];

            Vertices = new List<Vertex>();
            Edges = new List<Edge>();
            Cells = new Dictionary<int, Cell>();
        }

        public void Reset()
        {
            //if (!this.beachline)
            //{
            //    this.beachline = new this.RBTree();
            //}
            //// Move leftover beachsections to the beachsection junkyard.
            //if (this.beachline.root)
            //{
            //    var beachsection = this.beachline.getFirst(this.beachline.root);
            //    while (beachsection)
            //    {
            //        this.beachsectionJunkyard.push(beachsection); // mark for reuse
            //        beachsection = beachsection.rbNext;
            //    }
            //}
            //this.beachline.root = null;
            //if (!this.circleEvents)
            //{
            //    this.circleEvents = new this.RBTree();
            //}
            //this.circleEvents.root = this.firstCircleEvent = null;
            //this.vertices = [];
            //this.edges = [];
            //this.cells = [];

            BeachLine = new RBTree();
            CircleEvents = new RBTree();

            BeachLine.Name = "BeachLine";
            CircleEvents.Name = "CircleEvents";

            firstCircleEvent = null;

            Vertices.Clear();
            Edges.Clear();
            Cells.Clear();
        }

        public void GenerateSites(int n, int width, int height)
        {
            //this.randomSites(n);
            //this.reset();
            //this.processQueueAll();

            RandomSites(n, width, height);
            //Reset();
            Compute(Input);
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

            Input.Add(new Site(659, 246));
            Input.Add(new Site(643, 265));
            Input.Add(new Site(307, 406));
            Input.Add(new Site(571, 18));
            Input.Add(new Site(47, 180));
            Input.Add(new Site(276, 511));
            Input.Add(new Site(654, 190));
            Input.Add(new Site(12, 31));
            Input.Add(new Site(20, 598));
            Input.Add(new Site(446, 114));

            for (int i = 0; i < n; i++)
            {
                //var s = new Site(r.Next(0, width), r.Next(0, height));
                //var item = new QueueItem(s);
                var s = Input[i];

                //Sites.Add(item);

                Console.WriteLine("INPUT POINT #" + i);
                Console.WriteLine("X=" + s.X + ", Y=" + s.Y);

                //Input.Add(s);
            }
        }

        private int sitesPopped = 0;

        private Site PopSite()
        {
            if (Sites.Count == 0)
                return null;

            //var site = Sites[Sites.Count - 1];
            //Sites.RemoveAt(Sites.Count - 1);

            sitesPopped++;

            var site = Sites[0];
            Sites.RemoveAt(0);

            Console.WriteLine();
            Console.WriteLine("* POPPED #" + sitesPopped + ": " + site);
            Console.WriteLine("* SITES REMAINING: " + Sites.Count);

            return site;
        }

        public void Compute(List<Site> input)
        {
            //this.reset();
            Reset();

            //// any diagram data available for recycling?
            //// I do that here so that this is included in execution time
            //if (this.toRecycle)
            //{
            //    this.vertexJunkyard = this.vertexJunkyard.concat(this.toRecycle.vertices);
            //    this.edgeJunkyard = this.edgeJunkyard.concat(this.toRecycle.edges);
            //    this.cellJunkyard = this.cellJunkyard.concat(this.toRecycle.cells);
            //    this.toRecycle = null;
            //}

            //// Initialize site event queue
            //var siteEvents = sites.slice(0);
            //siteEvents.sort(function(a, b){
            //    var r = b.y - a.y;
            //    if (r) { return r; }
            //    return b.x - a.x;
            //});
            Sites = new List<Site>();

            Console.WriteLine("+++ INITIALIZING WITH " + input.Count + " SITES");
            for (int i = 0; i < input.Count; i++)
            {
                var s = Input[i];

                Sites.Add(s);
            }

            Sites.Sort();

            //// process queue
            //var site = siteEvents.pop(),
            //    siteid = 0,
            //    xsitex, // to avoid duplicate sites
            //    xsitey,
            //    cells = this.cells,
            //    circle;

            var site = PopSite();
            double xsitex = -1, xsitey = -1;
            CircleEvent circle = null;

            //// main loop
            //for (; ; )
            //{
            //    // we need to figure whether we handle a site or circle event
            //    // for this we find out if there is a site event and it is
            //    // 'earlier' than the circle event
            //    circle = this.firstCircleEvent;
            for(; ;)
            {
                circle = firstCircleEvent;
                //circle = firstCircleEvent == null ? null : firstCircleEvent.Arc;

                if(site != null && (circle == null 
                                 || site.Y < circle.Y 
                                 || (site.Y == circle.Y && site.X < circle.X)
                                   )
                  )
                {
                    if(site.X != xsitex || site.Y != xsitey)
                    {
                        //GetCell(site.Id) = CreateCell(site);
                        Cells.Add(site.Id, CreateCell(site));
                        AddBeachSection(site);
                        xsitex = site.X;
                        xsitey = site.Y;
                    }

                    site = PopSite();
                } else if(circle != null)
                {
                    //RemoveBeachSection(circle.BeachSection, circle);
                    //RemoveBeachSection(circle.CircleEvent.Arc);
                    RemoveBeachSection(circle);
                } else
                {
                    break;
                }
            }

            //    // add beach section
            //    if (site && (!circle || site.y < circle.y || (site.y === circle.y && site.x < circle.x)))
            //    {
            //        // only if site is not a duplicate
            //        if (site.x !== xsitex || site.y !== xsitey)
            //        {
            //            // first create cell for new site
            //            cells[siteid] = this.createCell(site);
            //            site.voronoiId = siteid++;
            //            // then create a beachsection for that site
            //            this.addBeachsection(site);
            //            // remember last site coords to detect duplicate
            //            xsitey = site.y;
            //            xsitex = site.x;
            //        }
            //        site = siteEvents.pop();
            //    }

            //    // remove beach section
            //    else if (circle)
            //    {
            //        this.removeBeachsection(circle.arc);
            //    }

            //    // all done, quit
            //    else
            //    {
            //        break;
            //    }
            //}
        }

        private CircleEvent firstCircleEvent;
        //private TreeNode firstCircleEventNode;
        //private TreeNode firstCircleEvent;

        //    EPSILON: 1e-4,
        //equalWithEpsilon: function(a, b) { return this.abs(a - b) < 1e-4; },
        //greaterThanWithEpsilon: function(a, b) { return (a - b) > 1e-4; },
        //greaterThanOrEqualWithEpsilon: function(a, b) { return (b - a) < 1e-4; },
        //lessThanWithEpsilon: function(a, b) { return (b - a) > 1e-4; },

        private static double epsilon = 1e-4;

        public bool equals(double a, double b) { return Math.Abs(a - b) < epsilon; }
        public bool gt(double a, double b) { return (a - b) > epsilon; }
        public bool gte(double a, double b) { return (b - a) < epsilon; }
        public bool lt(double a, double b) { return (b - a) > epsilon; }

        public Cell CreateCell(Site site)
        {
            return new Cell(site);
        }

        public HalfEdge CreateHalfEdge(Edge edge, Site leftSite, Site rightSite)
        {
            return new HalfEdge(edge, leftSite, rightSite);
        }

        public Vertex CreateVertex(double x, double y)
        {
            var v = new Vertex(x, y);

            Vertices.Add(v);

            return v;
        }

        private Cell GetCell(int index, Site site)
        {
            if (!Cells.ContainsKey(index))
                Cells.Add(index, new Cell(site));

            return Cells[index];
        }

        public Edge CreateEdge(Site leftSite, Site rightSite, Vertex va, Vertex vb)
        {
            //var edge = this.edgeJunkyard.pop();
            //if (!edge)
            //{
            //    edge = new this.Edge(lSite, rSite);
            //}
            //else
            //{
            //    edge.lSite = lSite;
            //    edge.rSite = rSite;
            //    edge.va = edge.vb = null;
            //}

            //this.edges.push(edge);
            //if (va)
            //{
            //    this.setEdgeStartpoint(edge, lSite, rSite, va);
            //}
            //if (vb)
            //{
            //    this.setEdgeEndpoint(edge, lSite, rSite, vb);
            //}
            //this.cells[lSite.voronoiId].halfedges.push(this.createHalfedge(edge, lSite, rSite));
            //this.cells[rSite.voronoiId].halfedges.push(this.createHalfedge(edge, rSite, lSite));
            //return edge;

            var edge = new Edge { LeftSite = leftSite, RightSite = rightSite };

            Edges.Add(edge);

            if (va != null)
            {
                edge.SetStartPoint(leftSite, rightSite, va);
            }

            if (vb != null)
            {
                edge.SetEndPoint(leftSite, rightSite, vb);
            }

            GetCell(leftSite.Id, leftSite).HalfEdges.Add(CreateHalfEdge(edge, leftSite, rightSite));
            GetCell(rightSite.Id, rightSite).HalfEdges.Add(CreateHalfEdge(edge, rightSite, leftSite));

            return edge;
        }

        public Edge CreateBorderEdge(Site leftSite, Vertex va, Vertex vb)
        {
            //var edge = this.edgeJunkyard.pop();
            //if (!edge)
            //{
            //    edge = new this.Edge(lSite, null);
            //}
            //else
            //{
            //    edge.lSite = lSite;
            //    edge.rSite = null;
            //}
            //edge.va = va;
            //edge.vb = vb;
            //this.edges.push(edge);
            //return edge;

            var edge = new Edge { LeftSite = leftSite, RightSite = null };

            edge.VA = va;
            edge.VB = vb;
            Edges.Add(edge);

            return edge;
        }

        public BeachSection CreateBeachSection(Site site)
        {
            return new BeachSection { Site = site };
        }

        public double GetLeftBreakPoint(BeachSection arc, TreeNode arcNode, double dir)
        {
            /// start

            //    var site = arc.site,
            //        rfocx = site.x,
            //        rfocy = site.y,
            //        pby2 = rfocy - directrix;

            var site = arc.Site;
            double rfocx = site.X;
            double rfocy = site.Y;
            double pby2 = rfocy - dir;

            //    // parabola in degenerate case where focus is on directrix
            //    if (!pby2)
            //    {
            //        return rfocx;
            //    }

            if (pby2 == 0)
            {
                return rfocx;
            }

            //    var lArc = arc.rbPrevious;

            var lArc = arcNode.Previous;

            //    if (!lArc)
            //    {
            //        return -Infinity;
            //    }

            if (lArc == null)
            {
                return Double.MinValue;
            }

            //    site = lArc.site;
            //    var lfocx = site.x,
            //        lfocy = site.y,
            //        plby2 = lfocy - directrix;

            //site = (Site)(lArc.Data);
            site = lArc.BeachSection.Site; // lArc.Site is null, this Site isn't
            double lfocx = site.X;
            double lfocy = site.Y;
            double plby2 = lfocy - dir;

            //    // parabola in degenerate case where focus is on directrix
            //    if (!plby2)
            //    {
            //        return lfocx;
            //    }

            if (plby2 == 0)
            {
                return lfocx;
            }

            //    var hl = lfocx - rfocx,
            //        aby2 = 1 / pby2 - 1 / plby2,
            //        b = hl / plby2;
            //    if (aby2)
            //    {
            //        return (-b + this.sqrt(b * b - 2 * aby2 * (hl * hl / (-2 * plby2) - lfocy + plby2 / 2 + rfocy - pby2 / 2))) / aby2 + rfocx;
            //    }
            //    // both parabolas have same distance to directrix, thus break point is midway
            //    return (rfocx + lfocx) / 2;
            //};

            double hl = lfocx - rfocx;
            double aby2 = (1.0 / pby2) - (1.0 / plby2);
            double b = hl / plby2;

            if (aby2 != 0)
            {
                return (-b + Math.Sqrt(b * b - 2 * aby2 * (hl * hl / (-2 * plby2) - lfocy + plby2 / 2 + rfocy - pby2 / 2))) / aby2 + rfocx;
            }

            return (rfocx + lfocx) / 2;

            /// end
        }

        public double GetRightBreakPoint(BeachSection arc, TreeNode arcNode, double dir)
        {
            //Voronoi.prototype.rightBreakPoint = function(arc, directrix) {
            //    var rArc = arc.rbNext;
            //    if (rArc)
            //    {
            //        return this.leftBreakPoint(rArc, directrix);
            //    }
            //    var site = arc.site;
            //    return site.y === directrix ? site.x : Infinity;
            //};

            var rArc = arcNode.Next;

            if(rArc != null)
            {
                //return GetLeftBreakPoint((BeachSection)rArc.Data, arcNode, dir);
                return GetLeftBreakPoint(rArc.BeachSection, rArc, dir);
            }

            var site = arc.Site;

            return site.Y == dir ? site.X : Double.MaxValue;
        }

        public void DetachBeachSection(TreeNode beachSectionNode)
        {
            //this.detachCircleEvent(beachsection); // detach potentially attached circle event
            //this.beachline.rbRemoveNode(beachsection); // remove from RB-tree
            //this.beachsectionJunkyard.push(beachsection); // mark for reuse

            DetachCircleEvent(beachSectionNode);
            BeachLine.RemoveNode(beachSectionNode);

            //BeachLine.Dump("BeachLine removing node", beachLineCounter++);
            BeachLine.Dump("detachBeachSection, remove", beachLineCounter++);
            //dumpTree(this.beachline, "detachBeachSection, remove", beachLineCounter);
            beachLineCounter++;
        }

        public void RemoveBeachSection(CircleEvent circle) //TreeNode beachSectionNode) //BeachSection b, TreeNode beachSectionNode)
        {
            //var circle = beachsection.circleEvent,
            //    x = circle.x,
            //    y = circle.ycenter,
            //    vertex = this.createVertex(x, y),
            //    previous = beachsection.rbPrevious,
            //    next = beachsection.rbNext,
            //    disappearingTransitions = [beachsection],
            //    abs_fn = Math.abs;

            //var circle = b.CircleEvent.CircleEvent;
            //var circle = beachSectionNode.CircleEvent;
            var beachSectionNode = circle.Arc;
            var x = circle.X;
            var y = circle.CenterY;
            var vertex = CreateVertex(x, y);
            var previous = beachSectionNode.Previous;
            var next = beachSectionNode.Next;
            //var disappearingTransitions = new List<BeachSection>();
            var disappearingTransitions = new List<TreeNode>();
            disappearingTransitions.Add(beachSectionNode);

            //this.detachBeachsection(beachsection);
            DetachBeachSection(beachSectionNode);

            //var lArc = previous;
            //while (lArc.circleEvent && abs_fn(x - lArc.circleEvent.x) < 1e-9 && abs_fn(y - lArc.circleEvent.ycenter) < 1e-9)
            //{
            //    previous = lArc.rbPrevious;
            //    disappearingTransitions.unshift(lArc);
            //    this.detachBeachsection(lArc); // mark for reuse
            //    lArc = previous;
            //}
            //disappearingTransitions.unshift(lArc);
            //this.detachCircleEvent(lArc);

            var lArc = previous;
            while(lArc.CircleEvent != null
                && Math.Abs(x - lArc.CircleEvent.X) < epsilon 
                && Math.Abs(y - lArc.CircleEvent.CenterY) < epsilon)
            {
                previous = lArc.Previous;
                disappearingTransitions.Insert(0, lArc);
                //DetachBeachSection(lArc.BeachSection, lArc);
                DetachBeachSection(lArc);
                lArc = previous;
            }
            disappearingTransitions.Insert(0, lArc);
            DetachCircleEvent(lArc);

            //// look right
            //var rArc = next;
            //while (rArc.circleEvent && abs_fn(x - rArc.circleEvent.x) < 1e-9 && abs_fn(y - rArc.circleEvent.ycenter) < 1e-9)
            //{
            //    next = rArc.rbNext;
            //    disappearingTransitions.push(rArc);
            //    this.detachBeachsection(rArc); // mark for reuse
            //    rArc = next;
            //}
            //disappearingTransitions.push(rArc);
            //this.detachCircleEvent(rArc);

            var rArc = next;
            while(rArc.CircleEvent != null
                && Math.Abs(x - rArc.CircleEvent.X) < epsilon 
                && Math.Abs(y - rArc.CircleEvent.CenterY) < epsilon)
            {
                next = rArc.Next;
                disappearingTransitions.Add(rArc);
                //DetachBeachSection(rArc.BeachSection, rArc);
                DetachBeachSection(rArc);
                rArc = next;
            }
            disappearingTransitions.Add(rArc);
            DetachCircleEvent(rArc);

            //var nArcs = disappearingTransitions.length,
            //    iArc;
            //for (iArc = 1; iArc < nArcs; iArc++)
            //{
            //    rArc = disappearingTransitions[iArc];
            //    lArc = disappearingTransitions[iArc - 1];
            //    this.setEdgeStartpoint(rArc.edge, lArc.site, rArc.site, vertex);
            //}

            var nArcs = disappearingTransitions.Count;
            int iArc;
            for(iArc = 1; iArc < nArcs; iArc++)
            {
                rArc = disappearingTransitions[iArc];
                lArc = disappearingTransitions[iArc - 1];
                rArc.BeachSection.Edge.SetStartPoint(lArc.BeachSection.Site, rArc.BeachSection.Site, vertex);
            }

            //lArc = disappearingTransitions[0];
            //rArc = disappearingTransitions[nArcs - 1];
            //rArc.edge = this.createEdge(lArc.site, rArc.site, undefined, vertex);

            lArc = disappearingTransitions[0];
            rArc = disappearingTransitions[nArcs - 1];
            rArc.BeachSection.Edge = CreateEdge(lArc.BeachSection.Site, rArc.BeachSection.Site, null, vertex);

            //this.a--ttachCircleEvent(lArc);
            //this.a--ttachCircleEvent(rArc);

            AttachCircleEvent(lArc);
            AttachCircleEvent(rArc);
        }

        private int beachLineCounter = 0;
        private int circleEventCounter = 0;
        public void AddBeachSection(Site site)
        {
            //var x = site.x,
            //    directrix = site.y;

            var x = site.X;
            var dir = site.Y;

            //var lArc, rArc,
            //    dxl, dxr,
            //    node = this.beachline.root;

            TreeNode lArc = null, rArc = null;
            double dxl, dxr;
            TreeNode node = BeachLine.root;

            while(node != null)
            {
                //    dxl = this.leftBreakPoint(node, directrix) - x;
                dxl = GetLeftBreakPoint(node.BeachSection, node, dir) - x;

                //    // x lessThanWithEpsilon xl => falls somewhere before the left edge of the beachsection
                //    if (dxl > 1e-9)
                //    {
                //        node = node.rbLeft;
                //    }
                //    else
                //    {
                if (dxl > epsilon)
                {
                    node = node.Left;
                }
                else
                {
                    //        dxr = x - this.rightBreakPoint(node, directrix);
                    dxr = x - GetRightBreakPoint(node.BeachSection, node, dir);

                    //        // x greaterThanWithEpsilon xr => falls somewhere after the right edge of the beachsection
                    //        if (dxr > 1e-9)
                    //        {
                    //            if (!node.rbRight)
                    //            {
                    //                lArc = node;
                    //                break;
                    //            }
                    //            node = node.rbRight;
                    //        }
                    if (dxr > epsilon)
                    {
                        if(node.Right == null)
                        {
                            lArc = node;
                            break;
                        }

                        node = node.Right;
                    } else
                    {
                        if(dxl > -epsilon)
                        {
                            lArc = node.Previous;
                            rArc = node;
                        } else if(dxr > -epsilon)
                        {
                            lArc = node;
                            rArc = node.Next;
                        } else
                        {
                            lArc = node;
                            rArc = node;
                        }
                        break;
                    }

                    //        else
                    //        {
                    //            // x equalWithEpsilon xl => falls exactly on the left edge of the beachsection
                    //            if (dxl > -1e-9)
                    //            {
                    //                lArc = node.rbPrevious;
                    //                rArc = node;
                    //            }
                    //            // x equalWithEpsilon xr => falls exactly on the right edge of the beachsection
                    //            else if (dxr > -1e-9)
                    //            {
                    //                lArc = node;
                    //                rArc = node.rbNext;
                    //            }
                    //            // falls exactly somewhere in the middle of the beachsection
                    //            else
                    //            {
                    //                lArc = rArc = node;
                    //            }
                    //            break;
                    //        }

                //    }
                }

            }

            //var newArc = this.createBeachsection(site);
            //this.beachline.rbInsertSuccessor(lArc, newArc);

            var newArc = CreateBeachSection(site);
            var newArcNode = new TreeNode { BeachSection = newArc, X = site.X, Y = site.Y, Site = site };
            BeachLine.InsertSuccessor(lArc, newArcNode);

            //dumpTree(this.beachline, "BeachLine, addBeachSection first insert", beachLineCounter);
            BeachLine.Dump("BeachLine, addBeachSection first insert", beachLineCounter++);

            //if (!lArc && !rArc)
            //{
            //    return;
            //}

            if(lArc == null && rArc == null)
            {
                return;
            }

            //if (lArc === rArc)
            //{
            //    this.detachCircleEvent(lArc);

            //    rArc = this.createBeachsection(lArc.site);
            //    this.beachline.rbInsertSuccessor(newArc, rArc);

            //    newArc.edge = rArc.edge = this.createEdge(lArc.site, newArc.site);

            //    this.a--ttachCircleEvent(lArc);
            //    this.a--ttachCircleEvent(rArc);
            //    return;
            //}

            if (lArc == rArc)
            {
                DetachCircleEvent(lArc);

                var s = lArc.BeachSection.Site;
                rArc = new TreeNode { BeachSection = CreateBeachSection(s), X = s.X, Y = s.Y, Site = s };
                BeachLine.InsertSuccessor(newArcNode, rArc);
                BeachLine.Dump("BeachLine, addBeachSection second insert", beachLineCounter++);

                newArc.Edge = CreateEdge(lArc.BeachSection.Site, newArc.Site, null, null);
                rArc.BeachSection.Edge = newArc.Edge;

                AttachCircleEvent(lArc);
                AttachCircleEvent(rArc);
                return;
            }

            //if (lArc && !rArc)
            //{
            //    newArc.edge = this.createEdge(lArc.site, newArc.site);
            //    return;
            //}

            if(lArc != null && rArc == null)
            {
                newArc.Edge = CreateEdge(lArc.BeachSection.Site, newArc.Site, null, null);
                return;
            }

            ////if (!lArc && rArc) {
            ////    throw "Voronoi.addBeachsection(): What is this I don't even";
            ////    }
            ///
            if(lArc == null && rArc != null)
            {
                Console.WriteLine("SHOULD NOT HAPPEN");
            }

            //if (lArc !== rArc)
            //{
            //    this.detachCircleEvent(lArc);
            //    this.detachCircleEvent(rArc);

            //    var lSite = lArc.site,
            //        ax = lSite.x,
            //        ay = lSite.y,
            //        bx = site.x - ax,
            //        by = site.y - ay,
            //        rSite = rArc.site,
            //        cx = rSite.x - ax,
            //        cy = rSite.y - ay,
            //        d = 2 * (bx * cy - by * cx),
            //        hb = bx * bx + by * by,
            //        hc = cx * cx + cy * cy,
            //        vertex = this.createVertex((cy * hb - by * hc) / d + ax, (bx * hc - cx * hb) / d + ay);

            //    this.setEdgeStartpoint(rArc.edge, lSite, rSite, vertex);

            //    newArc.edge = this.createEdge(lSite, site, undefined, vertex);
            //    rArc.edge = this.createEdge(site, rSite, undefined, vertex);

            //    this.a--ttachCircleEvent(lArc);
            //    this.a--ttachCircleEvent(rArc);
            //    return;
            //}

            if(lArc != rArc)
            {
                DetachCircleEvent(lArc);
                DetachCircleEvent(rArc);

                var lSite = lArc.BeachSection.Site;
                var ax = lSite.X;
                var ay = lSite.Y;
                var bx = site.X - ax;
                var by = site.Y - ay;
                var rSite = rArc.BeachSection.Site;
                var cx = rSite.X - ax;
                var cy = rSite.Y - ay;
                var d = 2 * (bx * cy - by * cx);
                var hb = bx * bx + by * by;
                var hc = cx * cx + cy * cy;
                var vertex = CreateVertex((cy * hb - by * hc) / d + ax, (bx * hc - cx * hb) / d + ay);

                rArc.BeachSection.Edge = CreateEdge(lSite, site, null, vertex);

                newArc.Edge = CreateEdge(lSite, site, null, vertex);
                rArc.BeachSection.Edge = CreateEdge(site, rSite, null, vertex);

                AttachCircleEvent(lArc);
                AttachCircleEvent(rArc);
                return;
            }
        }

        public void AttachCircleEvent(TreeNode n)
        {
            // start

            //        var lArc = arc.rbPrevious,
            //rArc = arc.rbNext;
            //        if (!lArc || !rArc) { return; } // does that ever happen?
            //        var lSite = lArc.site,
            //            cSite = arc.site,
            //            rSite = rArc.site;

            var lArc = n.Previous;
            var rArc = n.Next;

            if(lArc == null || rArc == null)
            {
                Console.WriteLine("SHOULD NOT HAPPEN");
                return;
            }

            var lSite = lArc.BeachSection.Site;
            var cSite = n.BeachSection.Site;
            var rSite = rArc.BeachSection.Site;

            //        if (lSite === rSite) { return; }

            if(lSite.X == rSite.X && lSite.Y == rSite.Y)
            {
                return;
            }

            //        var bx = cSite.x,
            //            by = cSite.y,
            //            ax = lSite.x - bx,
            //            ay = lSite.y - by,
            //            cx = rSite.x - bx,
            //            cy = rSite.y - by;

            var bx = cSite.X;
            var by = cSite.Y;
            var ax = lSite.X - bx;
            var ay = lSite.Y - by;
            var cx = rSite.X - bx;
            var cy = rSite.Y - by;

            //        var d = 2 * (ax * cy - ay * cx);
            //        if (d >= -2e-12) { return; }

            var d = 2 * (ax * cy - ay * cx);
            if (d >= -2e-12)
                return;

            //        var ha = ax * ax + ay * ay,
            //            hc = cx * cx + cy * cy,
            //            x = (cy * ha - ay * hc) / d,
            //            y = (ax * hc - cx * ha) / d,
            //            ycenter = y + by;

            var ha = ax * ax + ay * ay;
            var hc = cx * cx + cy * cy;
            var x = (cy * ha - ay * hc) / d;
            var y = (ax * hc - cx * ha) / d;
            var ycenter = y + by;

            //        var circleEvent = this.circleEventJunkyard.pop();
            //        if (!circleEvent)
            //        {
            //            circleEvent = new this.CircleEvent();
            //        }

            var circleEvent = new CircleEvent();

            //        circleEvent.arc = arc;
            //        circleEvent.site = cSite;
            //        circleEvent.x = x + bx;
            //        circleEvent.y = ycenter + this.sqrt(x * x + y * y); // y bottom
            //        circleEvent.ycenter = ycenter;
            //        arc.circleEvent = circleEvent;

            circleEvent.Arc = n;
            circleEvent.Site = cSite;
            circleEvent.X = x + bx;
            circleEvent.Y = ycenter + Math.Sqrt(x * x + y * y);
            circleEvent.CenterY = ycenter;

            n.CircleEvent = circleEvent;

            //n.X = circleEvent.X; // probably remove
            //n.Y = circleEvent.Y; // probably remove

            TreeNode predecessor = null;
            TreeNode node = CircleEvents.root;

            //        var predecessor = null,
            //            node = this.circleEvents.root;
            //        while (node)
            //        {
            //            if (circleEvent.y < node.y || (circleEvent.y === node.y && circleEvent.x <= node.x))
            //            {
            //                if (node.rbLeft)
            //                {
            //                    node = node.rbLeft;
            //                }
            //                else
            //                {
            //                    predecessor = node.rbPrevious;
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                if (node.rbRight)
            //                {
            //                    node = node.rbRight;
            //                }
            //                else
            //                {
            //                    predecessor = node;
            //                    break;
            //                }
            //            }
            //        }

            while(node != null)
            {
                if(circleEvent.Y < node.Y || (circleEvent.Y == node.Y && circleEvent.X <= node.X))
                {
                    if(node.Left != null)
                    {
                        node = node.Left;
                    } else
                    {
                        predecessor = node.Previous;
                        break;
                    }
                } else
                {
                    if(node.Right != null)
                    {
                        node = node.Right;
                    } else
                    {
                        predecessor = node;
                        break;
                    }
                }
            }

            //        this.circleEvents.rbInsertSuccessor(predecessor, circleEvent);
            //        if (!predecessor)
            //        {
            //            this.firstCircleEvent = circleEvent;
            //        }

            var circleNode = new TreeNode { BeachSection = n.BeachSection, CircleEvent = circleEvent, X = circleEvent.X, Y = circleEvent.Y };
            CircleEvents.InsertSuccessor(predecessor, circleNode);

            n.CircleEventNode = circleNode;

            //dumpTree(this.circleEvents, "circleEvents, attachCircleEvent insert", circleEventCounter);
            CircleEvents.Dump("circleEvents, attachCircleEvent insert", circleEventCounter++);

            if (predecessor == null)
            {
                //firstCircleEvent = circleNode;
                //firstCircleEventNode = circleNode;
                //firstCircleEvent = circleEvent;
                firstCircleEvent = circleNode.CircleEvent;

                //if(circleNode.CircleEvent == null)
                //{
                //    Console.WriteLine("ERROR");
                //}
            }
            // end
        }

        public void DetachCircleEvent(TreeNode arc)
        {
            //var circleEvent = arc.circleEvent;
            //if (circleEvent)
            //{
            //    if (!circleEvent.rbPrevious)
            //    {
            //        this.firstCircleEvent = circleEvent.rbNext;
            //    }
            //    this.circleEvents.rbRemoveNode(circleEvent); // remove from RB-tree
            //    this.circleEventJunkyard.push(circleEvent);
            //    arc.circleEvent = null;
            //}

            //var circleEvent = n.AsCircleEvent();
            //var circleEvent = n.BeachSection.Site;
            var circleEvent = arc.CircleEvent;

            if (circleEvent != null)
            {
                if(arc.CircleEventNode.Previous == null)
                {
                    firstCircleEvent = arc.Next.CircleEvent;

                    //firstCircleEventNode = n.Next;

                    //if(firstCircleEvent.CircleEvent == null)
                    //{
                    //    Console.WriteLine("ERROR2");
                    //}
                }

                CircleEvents.RemoveNode(arc.CircleEventNode);
                CircleEvents.Dump("circleEvents, detachCircleEvent remove", circleEventCounter++);

                arc.CircleEvent = null;
                arc.CircleEventNode = null;
            }
        }
    }
}