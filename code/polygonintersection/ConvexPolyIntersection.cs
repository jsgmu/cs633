using CompGeomVis.algotracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.polygonintersection
{
    // This code is heavily based on the implementation provided by Joseph O'Rourke
    // in the book "Computational Geometry in C" second edition
    public class ConvexPolyIntersection : AlgorithmBase
    {
        public List<Vector> UnsortedPoly1 { get; set; }
        public List<Vector> UnsortedPoly2 { get; set; }
        public List<Vector> SortedPoly1 { get; set; }
        public List<Vector> SortedPoly2 { get; set; }
        public int N { get; set; }
        public int M { get; set; }

        private Vector LowestPoint;

        public ConvexPolyIntersection()
        {
            AlgorithmId = 50;
            History = new AlgorithmHistory();
        }

        public Vector FindLowestPoint(List<Vector> poly)
        {
            LowestPoint = null;

            foreach (var v in poly)
            {
                if (LowestPoint == null)
                {
                    LowestPoint = v;
                }
                else if (v.Y < LowestPoint.Y || (v.Y == LowestPoint.Y && v.X < LowestPoint.X))
                {
                    LowestPoint = v;
                }
            }

            return LowestPoint;
        }

        public int VectorCompare(Vector v1, Vector v2)
        {
            if (v1 == v2 || (v1.X == v2.X && v1.Y == v2.Y))
                return 0;

            double thetaA = Math.Atan2(v1.Y - LowestPoint.Y, v1.X - LowestPoint.X);
            double thetaB = Math.Atan2(v2.Y - LowestPoint.Y, v2.X - LowestPoint.X);

            // Angle of difference vector v1 and LowestPoint, and the x-axis
            // If v1 has the smaller angle, return -1
            if (thetaA < thetaB)
            {
                return -1;
            }
            // If v2 has the smaller angle, return 1
            else if (thetaA > thetaB)
            {
                return 1;
            }

            // If angles are the same, then choose the point that is closer to LowestPoint

            double distanceA = GeomMath.Distance(LowestPoint, v1);
            double distanceB = GeomMath.Distance(LowestPoint, v2);

            if (distanceA < distanceB)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        // Polygon points are sorted by smallest angle of difference vectors (to LowestPoint) and x-axis
        // Equal angles are tie-broken by smaller linear distance to LowerPoint
        private void SortPolygons()
        {
            LowestPoint = FindLowestPoint(UnsortedPoly1);
            SortedPoly1 = AlgorithmUtil.CopyVectorList(UnsortedPoly1);
            SortedPoly1.Sort(VectorCompare);

            LowestPoint = FindLowestPoint(UnsortedPoly2);
            SortedPoly2 = AlgorithmUtil.CopyVectorList(UnsortedPoly2);
            SortedPoly2.Sort(VectorCompare);
        }

        public int AreaSign(Vector a, Vector b, Vector c)
        {
            double area2;

            area2 = (b.X - a.X) * (double)(c.Y - a.Y) -
                    (c.X - a.X) * (double)(b.Y - a.Y);

            if (area2 > 0.5) return 1;
            else if (area2 < -0.5) return -1;
            else return 0;
        }

        public bool Collinear(Vector a, Vector b, Vector c)
        {
            return AreaSign(a, b, c) == 0;
        }

        // returns true if "c" is on a line ab and is between a and b
        // line ab cannot be vertical
        public bool Between(Vector a, Vector b, Vector c)
        {
            if(a.X != b.X)
            {
                return (a.X <= c.X && c.X <= b.X) || (a.X >= c.X && c.X >= b.X);
            }

            return (a.Y <= c.Y && c.Y <= b.Y) || (a.Y >= c.Y && c.Y >= b.Y);
        }

        public void Assign(Vector p, Vector a)
        {
            p.X = a.X;
            p.Y = a.Y;
        }

        public const int STATUS_UNKNOWN = -1;
        public const int STATUS_0 = 0;
        public const int STATUS_1 = 1;
        public const int STATUS_E = 2;
        public const int STATUS_V = 3;

        public const int INFLAG_UNKNOWN = -1;
        public const int INFLAG_PIN = 0;
        public const int INFLAG_QIN = 1;

        // Return values:
        //   0: if points a,b,c are not collinear
        //   e: c is on line ab and is between the points a and b (and p becomes c)
        //   e: d is on line ab and is between the points a and b (and p becomes d)
        //   e: a is on line cd and is between the points c and d (and p becomes a)
        //   e: b is on line cd and is between the points c and d (and p becomes b)
        //   0: default return value
        public int ParallelInt(Vector a, Vector b, Vector c, Vector d, Vector p)
        {
            //    if (!Collinear(a, b, c))
            //        return '0';
            if (!Collinear(a, b, c))
                return STATUS_0;

            //    if (Between(a, b, c))
            //    {
            //        Assigndi(p, c);
            //        return 'e';
            //    }

            if(Between(a, b, c))
            {
                Assign(p, c);
                return STATUS_E;
            }

            //    if (Between(a, b, d))
            //    {
            //        Assigndi(p, d);
            //        return 'e';
            //    }

            if(Between(a, b, d))
            {
                Assign(p, d);
                return STATUS_E;
            }

            //    if (Between(c, d, a))
            //    {
            //        Assigndi(p, a);
            //        return 'e';
            //    }
            if(Between(c, d, a))
            {
                Assign(p, a);
                return STATUS_E;
            }

            //    if (Between(c, d, b))
            //    {
            //        Assigndi(p, b);
            //        return 'e';
            //    }
            if (Between(c, d, b))
            {
                Assign(p, b);
                return STATUS_E;
            }

            return STATUS_0;
        }

        private string TranslateCode(int code)
        {
            if(code == STATUS_E)
            {
                return "The segments share a point and overlap collinearly";
            }

            if(code == STATUS_V)
            {
                return "One vertex is shared between the two segments but the segments do not overlap collinearly";
            }

            if(code == STATUS_1)
            {
                return "The segments intersect properly";
            }

            if(code == STATUS_0)
            {
                return "The segments do not intersect because they share no points";
            }

            return "Unknown";
        }

        /*---------------------------------------------------------------------
        SegSegInt: Finds the point of intersection p between two closed
        segments ab and cd.  Returns p and a char with the following meaning:
           'e': The segments collinearly overlap, sharing a point.
           'v': An endpoint (vertex) of one segment is on the other segment,
                but 'e' doesn't hold.
           '1': The segments intersect properly (i.e., they share a point and
                neither 'v' nor 'e' holds).
           '0': The segments do not intersect (i.e., they share no points).
        Note that two collinear segments that share just one point, an endpoint
        of each, returns 'e' rather than 'v' as one might expect.
        ---------------------------------------------------------------------*/
        public int SegSegInt(Vector a, Vector b, Vector c, Vector d, Vector p, Vector q)
        {
            //double s, t;       /* The two parameters of the parametric eqns. */
            //double num, denom;  /* Numerator and denoninator of equations. */
            //char code = '?';    /* Return char characterizing intersection. */
            double s, t;
            double num, denom;
            int code = STATUS_UNKNOWN;

            //denom = a[X] * (double)(d[Y] - c[Y]) +
            //        b[X] * (double)(c[Y] - d[Y]) +
            //        d[X] * (double)(b[Y] - a[Y]) +
            //        c[X] * (double)(a[Y] - b[Y]);
            denom = a.X * (d.Y - c.Y) + b.X * (c.Y - d.Y) + d.X * (b.Y - a.Y) + c.X * (a.Y - b.Y);

            ///* If denom is zero, then segments are parallel: handle separately. */
            //if (denom == 0.0)
            //    return ParallelInt(a, b, c, d, p);
            if(denom == 0)
            {
                return ParallelInt(a, b, c, d, p); // <-- this should be fixed to use alternate ParallelInt to cover case where q must be set instea of p
            }

            //num = a[X] * (double)(d[Y] - c[Y]) +
            //         c[X] * (double)(a[Y] - d[Y]) +
            //         d[X] * (double)(c[Y] - a[Y]);
            //if ((num == 0.0) || (num == denom)) code = 'v';
            //s = num / denom;
            //printf("num=%lf, denom=%lf, s=%lf\n", num, denom, s);

            num = a.X * (d.Y - c.Y) + c.X * (a.Y - d.Y) + d.X * (c.Y - a.Y);

            if (num == 0 || num == denom)
                code = STATUS_V;

            s = num / denom;

            //num = -(a[X] * (double)(c[Y] - b[Y]) +
            //         b[X] * (double)(a[Y] - c[Y]) +
            //         c[X] * (double)(b[Y] - a[Y]));
            //if ((num == 0.0) || (num == denom)) code = 'v';
            //t = num / denom;
            //printf("num=%lf, denom=%lf, t=%lf\n", num, denom, t);

            num = -(a.X * (c.Y - b.Y) + b.X * (a.Y - c.Y) + c.X * (b.Y - a.Y));

            if (num == 0 || num == denom)
                code = STATUS_V;

            t = num / denom;

            //if ((0.0 < s) && (s < 1.0) &&
            //          (0.0 < t) && (t < 1.0))
            //    code = '1';
            //else if ((0.0 > s) || (s > 1.0) ||
            //          (0.0 > t) || (t > 1.0))
            //    code = '0';

            if (0 < s && s < 1.0 && 0 < t && t < 1.0)
                code = STATUS_1;
            else if (0 > s || s > 1.0 || 0 > t || t > 1.0)
                code = STATUS_0;

            //p[X] = a[X] + s * (b[X] - a[X]);
            //p[Y] = a[Y] + s * (b[Y] - a[Y]);

            p.X = a.X + s * (b.X - a.X);
            p.Y = a.Y + s * (b.Y - a.Y);

            //return code;
            return code;
        }

        public int InOut(Vector tp, int inFlag, int aHB, int bHA)
        {
            // Lineto p
            //LineTo(tp); // done at caller

            if (aHB > 0)
                return INFLAG_PIN;
            else if (bHA > 0)
                return INFLAG_QIN;

            return inFlag;
        }

        private string TranslateInOut(int flag)
        {
            if (flag == INFLAG_PIN)
                return "In P";

            if (flag == INFLAG_QIN)
                return "In Q";

            return "Unknown";
        }

        private Vector currentStartPoint;
        public PolygonModel IntersectingPolygon { get; set; }

        private void MoveTo(Vector p)
        {
            Console.WriteLine("MoveTo: " + p);
            currentStartPoint = new Vector { X = p.X, Y = p.Y };
        }

        private void LineTo(Vector p)
        {
            //Console.WriteLine("LineTo: " + p);

            if (IntersectingPolygon == null)
                IntersectingPolygon = new PolygonModel();

            if(!GeomMath.AlmostEqual(currentStartPoint, p))
            {
                IntersectingPolygon.Lines.Add(new LineModel {
                    StartPoint = new Vector { X = currentStartPoint.X, Y = currentStartPoint.Y },
                    EndPoint = new Vector { X = p.X, Y = p.Y }
                });
                currentStartPoint = new Vector { X = p.X, Y = p.Y };
                //currentStartPoint = p; // point becomes start of next line
            }
        }

        private AlgorithmStatusLayer activeStatusLayer = null;

        public void Compute()
        {
            SortPolygons();

            List<Vector> P = SortedPoly1;
            List<Vector> Q = SortedPoly2;

            int n = SortedPoly1.Count;
            int m = SortedPoly2.Count;
            int a, b;
            int a1, b1;
            Vector A = new Vector(), B = new Vector();
            int cross;
            int bHA, aHB;
            Vector origin = new Vector { X = 0, Y = 0 };
            Vector p = new Vector(), q = new Vector();
            int inFlag;
            int aa, ba;
            bool firstPoint;
            Vector p0 = new Vector();
            int code;

            a = 0; b = 0; aa = 0; ba = 0;
            inFlag = INFLAG_UNKNOWN;
            firstPoint = true;

            activeStatusLayer = History.CreateAndAddNewLayer("Setup");
            activeStatusLayer.AddCommand(new AddTextStatusCommand {
                AssociatedAlgorithm = this,
                Comments = "The 'in' flag starts as unknown, initial vectors start at index 0"
            });

            do
            {
                a1 = (a + n - 1) % n;
                b1 = (b + m - 1) % m;

                activeStatusLayer = History.CreateAndAddNewLayer("Main loop, a=" + a + ", b=" + b + ", a1=" + a1 + ", b1=" + b1);

                // SubVec(P[a], P[a1], A);
                A.X = P[a].X - P[a1].X;
                A.Y = P[a].Y - P[a1].Y;
                // SubVec(Q[b], Q[b1], B);
                B.X = Q[b].X - Q[b1].X;
                B.Y = Q[b].Y - Q[b1].Y;

                activeStatusLayer.AddCommand(new ClearTextStatusCommand { AssociatedAlgorithm = this });
                AddNonIndexedLineCommand(activeStatusLayer, new Vector { X = P[a1].X, Y = P[a1].Y }, new Vector { X = P[a].X, Y = P[a].Y });
                AddNonIndexedLineCommand(activeStatusLayer, new Vector { X = Q[b1].X, Y = Q[b1].Y }, new Vector { X = Q[b].X, Y = Q[b].Y });

                cross = AreaSign(origin, A, B);
                aHB = AreaSign(Q[b1], Q[b], P[a]);
                bHA = AreaSign(P[a1], P[a], Q[b]);

                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "cross=" + cross
                });
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "aHB=" + aHB
                });
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "bHA=" + bHA
                });

                code = SegSegInt(P[a1], P[a], Q[b1], Q[b], p, q);

                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Segment intersection status: " + TranslateCode(code)
                });

                if (code == STATUS_1 || code == STATUS_V)
                {
                    if(inFlag == STATUS_UNKNOWN && firstPoint)
                    {
                        aa = 0;
                        ba = 0;
                        firstPoint = false;
                        p0.X = p.X;
                        p0.Y = p.Y;
                        // moveto p0
                        MoveTo(p0);
                    }

                    LineTo(p);
                    inFlag = InOut(p, inFlag, aHB, bHA);

                    activeStatusLayer.AddCommand(new AddTextStatusCommand
                    {
                        AssociatedAlgorithm = this,
                        Comments = "Newly computed 'in' flag: " + TranslateInOut(inFlag)
                    });
                }

                if (code == STATUS_E && GeomMath.DotProduct(A, B) < 0)
                {
                    activeStatusLayer.AddCommand(new AddTextStatusCommand
                    {
                        AssociatedAlgorithm = this,
                        Comments = "Shared segement and ..."
                    });
                    // print shared segment
                    // return
                }

                if (cross == 0 && aHB < 0 && bHA < 0)
                {
                    // disjoint
                    // return
                    activeStatusLayer.AddCommand(new AddTextStatusCommand
                    {
                        AssociatedAlgorithm = this,
                        Comments = "Polygons are disjoint"
                    });
                }
                else if(cross == 0 && aHB == 0 && bHA == 0)
                {
                    // advance but do not output point
                    // page 262
                    if (inFlag == INFLAG_PIN)
                    {
                        // b = Advance(b, &ba, m, inflag == Qin, Q[b]);
                        // if inFlag == Qin // always false here
                        //    lineto Q[b]
                        ba++;
                        b = (b + 1) % m;
                        activeStatusLayer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Advancing b vector but not including in output as not part of interection"
                        });
                    }
                    else {
                        // a = Advance(a, &aa, n, inflag == Pin, P[a]);
                        // if inFlag == Pin // always false here
                        //    lineto P[a]
                        aa++;
                        a = (a + 1) % n;
                        activeStatusLayer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Advancing a vector but not including in output as not part of interection"
                        });
                    }
                }
                else if(cross >= 0)
                {
                    if(bHA > 0)
                    {
                        // if inflag == PIN
                        //    lineto P[a]
                        if(inFlag == INFLAG_PIN)
                        {
                            LineTo(P[a]);
                        }
                        aa++;
                        a = (a + 1) % n;
                        activeStatusLayer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Advancing a vector and including in output as part of interection"
                        });
                    }
                    else
                    {
                        // if inflag == QIN
                        //    lineto Q[b]
                        if (inFlag == INFLAG_QIN)
                        {
                            LineTo(Q[b]);
                        }
                        ba++;
                        b = (b + 1) % m;
                        activeStatusLayer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Advancing b vector and including in output as part of interection"
                        });
                    }
                } else
                {
                    // cross < 0
                    if(aHB > 0)
                    {
                        // if inflag == QIN
                        //    lineto Q[b]
                        if (inFlag == INFLAG_QIN)
                        {
                            LineTo(Q[b]);
                        }
                        ba++;
                        b = (b + 1) % m;
                        activeStatusLayer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Advancing b vector and including in output as part of interection"
                        });
                    }
                    else
                    {
                        // if inflag == PIN
                        //     lineto P[a]
                        if (inFlag == INFLAG_PIN)
                        {
                            LineTo(P[a]);
                        }
                        aa++;
                        a = (a + 1) % n;
                        activeStatusLayer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Advancing a vector and including in output as part of interection"
                        });
                    }
                }
            } while ( ((aa < n) || (ba < m)) && (aa < 2*n) && (ba < 2*m) );

            if(aa >= 2*n)
            {
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Termination condition met: aa >= 2*n"
                });
            }
            else if(aa >= n)
            {
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Termination condition met: aa >= n"
                });
            }

            if (ba >= 2 * m)
            {
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Termination condition met: ba >= 2*m"
                });
            }
            else if (ba >= m)
            {
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Termination condition met: ba >= m"
                });
            }

            if (!firstPoint)
            {
                // close intesection, line to p0
                LineTo(p0);
            }

            if (inFlag == INFLAG_UNKNOWN)
            {
                // boundaries do not cross
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Boundaries of polygons do not cross, no intersection"
                });
            }
        }

        public override void Run()
        {
            //public List<Vector> UnsortedPoly1 { get; set; }
            //public List<Vector> UnsortedPoly2 { get; set; }

            UnsortedPoly1 = new List<Vector>();

            foreach(var v in InputPolygons[0].Lines)
            {
                UnsortedPoly1.Add(new Vector { X = v.StartPoint.X, Y = v.StartPoint.Y, Alternates = v.StartPoint.Alternates });
            }

            UnsortedPoly2 = new List<Vector>();

            foreach (var v in InputPolygons[1].Lines)
            {
                UnsortedPoly2.Add(new Vector { X = v.StartPoint.X, Y = v.StartPoint.Y, Alternates = v.StartPoint.Alternates });
            }

            Compute();

            var layer = History.CreateAndAddNewLayer("Final Result");

            if (IntersectingPolygon.Lines == null || IntersectingPolygon.Lines.Count == 0)
            {
                activeStatusLayer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "No intersection of polygons"
                });
            }
            else {
                foreach (var line in IntersectingPolygon.Lines)
                {
                    layer.AddCommand(new HighlightNonIndexedLineCommand
                    {
                        StartX = line.StartPoint.X,
                        StartY = line.StartPoint.Y,
                        EndX = line.EndPoint.X,
                        EndY = line.EndPoint.Y
                    });
                }
            }
        }
    }
}
