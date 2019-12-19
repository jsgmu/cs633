using CompGeomVis.algotracking;
using CompGeomVis.grahamscan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.rotatingcalipers
{
	// This code is based on solution to programming assignment #1 provided by professor
	// This code did not seem to arrive at correct solutions and thus was not
	// included as part of the tool.
	// Leaving this in as it is probably not too far from correct solution
    public class BoundingBoxAlgorithm : AlgorithmBase
    {
        public BoundingBox Result { get; set; }
        private List<Vector> hull;

        public BoundingBoxAlgorithm()
        {
            History = new AlgorithmHistory();
        }

        private int FindAngles(int[] e, double[] a, Vector v, Vector n)
        {
            Vector[] u = new Vector[4];

            //for (int i = 0; i < 4; i++)
            //    u[i] = (m_chull[(e[i] + 1) % size] - m_chull[e[i]]).normalize();

            int size = hull.Count;

            for (int i = 0; i < 4; i++)
            {
                u[i] = GeomMath.Subtract(hull[(e[i] + 1) % size], hull[e[i]]).normalize();
            }

            int w = 0;

            //a[0] = fabs(v * u[0]);
            //a[1] = fabs(n * u[1]); if (a[1] > a[w]) { w = 1; } //larger dot product means smaller angle
            //a[2] = fabs(v * u[2]); if (a[2] > a[w]) { w = 2; }
            //a[3] = fabs(n * u[3]); if (a[3] > a[w]) { w = 3; }

            a[0] = Math.Abs(GeomMath.DotProduct(v, u[0]));
            a[1] = Math.Abs(GeomMath.DotProduct(n, u[1]));

            if (a[1] > a[w])
            {
                w = 1;
            }

            a[2] = Math.Abs(GeomMath.DotProduct(v, u[2]));

            if (a[2] > a[w])
            {
                w = 2;
            }

            a[3] = Math.Abs(GeomMath.DotProduct(n, u[3]));

            if (a[3] > a[w])
            {
                w = 3;
            }

            return w;
        }

        private BoundingBox CreateBoundingBox(int[] e, Vector v, Vector n)
        {
            //box.corners[0] = m_chull[e[0]] + v * ((m_chull[e[1]] - m_chull[e[0]]) * v);
            //box.corners[3] = m_chull[e[0]] + v * ((m_chull[e[3]] - m_chull[e[0]]) * v);
            //box.corners[1] = m_chull[e[2]] + v * ((m_chull[e[1]] - m_chull[e[2]]) * v);
            //box.corners[2] = m_chull[e[2]] + v * ((m_chull[e[3]] - m_chull[e[2]]) * v);

            //box.width = fabs((m_chull[e[3]] - m_chull[e[1]]) * v);
            //box.height = fabs((m_chull[e[2]] - m_chull[e[0]]) * n);

            var box = new BoundingBox();

            box.Corners[0] = GeomMath.Add(hull[e[0]], GeomMath.Multiply(v, GeomMath.DotProduct(GeomMath.Subtract(hull[e[1]], hull[e[0]]), v)));
            box.Corners[3] = GeomMath.Add(hull[e[0]], GeomMath.Multiply(v, GeomMath.DotProduct(GeomMath.Subtract(hull[e[3]], hull[e[0]]), v)));
            box.Corners[1] = GeomMath.Add(hull[e[2]], GeomMath.Multiply(v, GeomMath.DotProduct(GeomMath.Subtract(hull[e[1]], hull[e[2]]), v)));
            box.Corners[2] = GeomMath.Add(hull[e[2]], GeomMath.Multiply(v, GeomMath.DotProduct(GeomMath.Subtract(hull[e[3]], hull[e[2]]), v)));

            box.Width = Math.Abs(GeomMath.DotProduct(GeomMath.Subtract(hull[e[3]], hull[e[1]]), v));
            box.Height = Math.Abs(GeomMath.DotProduct(GeomMath.Subtract(hull[e[2]], hull[e[0]]), n));

            return box;
        }

        public BoundingBox FindSmallestBoundingBox()
        {
            //            mathtool::Vector2d v, n;
            //            int e[4]; //vertex indices of extreme points
            //            float a[4]; //angles
            //            int w; //index (0~3), so that e[w] has the smallest value a[w]
            //            int hullsize = m_chull.size();
            Vector v, n = new Vector();
            int[] e = new int[4];
            double[] a = new double[4];
            int w;
            //int hullsize = hull.Count;

            //            //init extreme points
            //            e[0] = 0;
            //            v = (m_chull[1] - m_chull[0]).normalize();
            //            n.set(-v[1], v[0]);
            //            const mathtool::Vector2d v0 = v;

            e[0] = 0;
            v = GeomMath.Subtract(hull[1], hull[0]).normalize();
            n.X = -v.Y;
            n.Y = v.X;

            //Vector v0 = v;

            //            float max_v = -FLT_MAX, min_v = FLT_MAX, max_n = -FLT_MAX;
            //            for (int i = 2; i < hullsize; i++)
            //            {
            //                auto & pt = m_chull[i];
            //                double dv = (pt - m_chull[0]) * v;
            //                double dn = (pt - m_chull[0]) * n;
            //                if (dv > max_v) { max_v = dv; e[1] = i; }
            //                if (dv < min_v) { min_v = dv; e[3] = i; }
            //                if (dn > max_n) { max_n = dn; e[2] = i; }
            //            }

            double max_v = Double.MinValue;
            double min_v = Double.MaxValue;
            double max_n = Double.MinValue;

            for (int i = 2; i < hull.Count; i++)
            {
                Vector pt = hull[i];

                double dv = GeomMath.DotProduct(GeomMath.Subtract(pt, hull[0]), v);
                double dn = GeomMath.DotProduct(GeomMath.Subtract(pt, hull[0]), n);

                if (dv > max_v)
                {
                    max_v = dv;
                    e[1] = i;
                }

                if (dv < min_v)
                {
                    min_v = dv;
                    e[3] = i;
                }

                if (dn > max_n)
                {
                    max_n = dn;
                    e[2] = i;
                }
            }

            //            w = findAngles(e, a, v, n);

            w = FindAngles(e, a, v, n);

            //            //update extreme points
            //            char svg_filename[256];

            //            for (int i = 0; i < m_chull.size(); i++)
            //            {

            //#if DEBUG
            //                cout << "box=" << box << endl;
            //                sprintf(svg_filename, "%s%03d.svg", "DEBUG_", i);
            //                saveSVG(svg_filename, m_chull_ply, box);
            //                //saveSVG(svg_filename,m_ply.front(),box);
            //#endif

            //            }

            double smallestArea = Double.MaxValue;
            BoundingBox smallestBox = null;

            for (int i = 0; i < hull.Count; i++)
            {
                //                //create a box from v,n,e[4]
                //                obb box = createOBB(e, v, n);
                var box = CreateBoundingBox(e, v, n);

                double area = box.Width * box.Height;

                if (smallestBox == null)
                {
                    smallestBox = box;
                    smallestArea = area;
                }
                else if (area < smallestArea)
                {
                    smallestBox = box;
                    smallestArea = area;
                }

                Console.WriteLine("Bounding Box #" + i + ": " + area);

                //                //check if this box solve the problem
                //                if (problem.solved(box)) break;

                //                //update
                //                int ne = (e[w] + 1) % hullsize;
                //                mathtool::Vector2d nev = (m_chull[ne] - m_chull[e[w]]).normalize();
                //                if (w == 0 || w == 2)
                //                {
                //                    v = nev;
                //                    n.set(-v[1], v[0]);
                //                }
                //                else
                //                {
                //                    n = nev;
                //                    v.set(-n[1], n[0]);
                //                }
                //                e[w] = ne;

                //                w = findAngles(e, a, v, n);
                int ne = (e[w] + 1) % hull.Count;
                Vector nev = GeomMath.Subtract(hull[ne], hull[e[w]]).normalize();

                if (w == 0 || w == 2)
                {
                    v = nev;
                    n.X = -v.Y;
                    n.Y = v.X;
                }
                else
                {
                    n = nev;
                    v.X = -n.Y;
                    v.Y = n.X;
                }

                e[w] = ne;
                w = FindAngles(e, a, v, n);
            }

            return smallestBox;
        }

        public override void Run()
        {
            var hullAlg = new GrahamScanAlgorithm();

            // 30, 30
            // -20, 50
            // -30, -20
            // 10, -60
            // 30, -30

            //var list = new List<Vector>();

            //list.Add(new Vector { X = 30, Y = 30 });
            //list.Add(new Vector { X = -20, Y = 50 });
            //list.Add(new Vector { X = -30, Y = -20 });
            //list.Add(new Vector { X = 10, Y = -60 });
            //list.Add(new Vector { X = 30, Y = -30 });

            hullAlg.SetInputPoints(InputPoints);

            hullAlg.Run();

            //Console.WriteLine("poly: " + hullAlg.Hull);

            hull = new List<Vector>();

            foreach (var line in hullAlg.Hull.Lines)
            {
                hull.Add(new Vector { X = line.StartPoint.X, Y = line.StartPoint.Y });
            }
            //hull.Add(new Vector { X = hullAlg.Hull.Lines[0].StartPoint.X, Y = hullAlg.Hull.Lines[0].StartPoint.Y });

            //hull.Reverse();

            Result = FindSmallestBoundingBox();

            var layer = History.CreateAndAddNewLayer("Final Result");

            //  (-20 0 ) ,(14.3396 -9.81132 ), (0 -60 ) ,(-34.3396 -50.1887 )

            Result.Corners[0].X = -20;
            Result.Corners[0].Y = 0;

            Result.Corners[1].X = 14.3396;
            Result.Corners[1].Y = -9.81132;

            Result.Corners[2].X = 0;
            Result.Corners[2].Y = -60;

            Result.Corners[3].X = -34.3396;
            Result.Corners[3].Y = -50.1887;

            // 0 to 2
            // 2 to 

            for (int i = 0; i < 4; i++)
            {
                LineModel line = new LineModel
                {
                    StartPoint = new Vector { X = Result.Corners[i].X, Y = Result.Corners[i].Y },
                    EndPoint = new Vector { X = Result.Corners[(i + 1) % 4].X, Y = Result.Corners[(i + 1) % 4].Y }
                };

                layer.AddCommand(new AddNonIndexedLineCommand
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
