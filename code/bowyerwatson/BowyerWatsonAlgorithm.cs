using CompGeomVis.algotracking;
using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.delaunay
{
    // Based on implementation from https://github.com/Bl4ckb0ne/delaunay-triangulation/
    // Also based on https://en.wikipedia.org/wiki/Bowyer%E2%80%93Watson_algorithm
    public class BowyerWatsonAlgorithm : AlgorithmBase
    {
        public List<DelaunayEdge> Edges { get; set; }
        public List<DelaunayTriangle> Triangles { get; set; }

        public BowyerWatsonAlgorithm()
        {
            Edges = new List<DelaunayEdge>();
            Triangles = new List<DelaunayTriangle>();

            AlgorithmId = 20;
            History = new AlgorithmHistory();
        }

        private void RemoveBadTriangles()
        {
            for(int j = Triangles.Count - 1; j >= 0; j--)
            {
                if (Triangles[j].IsBad)
                    Triangles.RemoveAt(j);
            }
        }

        private void RemoveTrianglesWithVertex(Vector p1, Vector p2, Vector p3)
        {
            for (int j = Triangles.Count - 1; j >= 0; j--)
            {
                if (Triangles[j].HasVertex(p1) || Triangles[j].HasVertex(p2) || Triangles[j].HasVertex(p3))
                    Triangles.RemoveAt(j);
            }
        }

        private void RemoveBadEdges()
        {
            for (int j = Edges.Count - 1; j >= 0; j--)
            {
                if (Edges[j].BadEdge)
                    Edges.RemoveAt(j);
            }
        }

        private void RemoveBadEdges(List<DelaunayEdge> edgeList)
        {
            for(int j = edgeList.Count - 1; j >= 0; j--)
            {
                if (edgeList[j].BadEdge)
                    edgeList.RemoveAt(j);
            }
        }

        private void AddTriangleLinesToLayer(AlgorithmStatusLayer layer)
        {
            for (int ti = 0; ti < Triangles.Count; ti++)
            {
                var tri = Triangles[ti];

                if (tri.IsBad)
                    continue;

                var v1 = tri.V1;
                var v2 = tri.V2;
                var v3 = tri.V3;

                AddLineCommand(layer, v1, v2);
                AddLineCommand(layer, v2, v3);
                AddLineCommand(layer, v3, v1);
            }
        }

        private void AddEdgesToLayer(AlgorithmStatusLayer layer)
        {
            for(int ei = 0; ei < Edges.Count; ei++)
            {
                var e = Edges[ei];

                if (e.BadEdge)
                    continue;

                var v1 = e.VStart;
                var v2 = e.VEnd;

                AddLineCommand(layer, v1, v2);
            }
        }

        public override void Run()
        {
            if (InputPoints == null || InputPoints.Count < 3)
            {
                return;
            }

            double minx = InputPoints[0].X;
            double miny = InputPoints[0].Y;
            double maxx = minx;
            double maxy = miny;

            for(int i = 0; i < InputPoints.Count; i++)
            {
                var v = InputPoints[i];

                if (v.X < minx) minx = v.X;
                if (v.Y < miny) miny = v.Y;
                if (v.X > maxx) maxx = v.X;
                if (v.Y > maxy) maxy = v.Y;
            }

            double dx = maxx - minx;
            double dy = maxy - miny;
            double deltaMax = Math.Max(dx, dy);
            double midx = 0.5 * (minx + maxx);
            double midy = 0.5 * (miny + maxy);

            Vector p1 = new Vector { X = midx - 20 * deltaMax, Y = midy - deltaMax };
            Vector p2 = new Vector { X = midx, Y = midy + 20 * deltaMax };
            Vector p3 = new Vector { X = midx + 20 * deltaMax, Y = midy - deltaMax };

            Triangles.Clear();
            Triangles.Add(new DelaunayTriangle { V1 = p1, V2 = p2, V3 = p3 });

            for (int i = 0; i < InputPoints.Count; i++)
            {
                Vector p = InputPoints[i];
                List<DelaunayEdge> polygon = new List<DelaunayEdge>();

                var badTrianglesLayer = History.CreateAndAddNewLayer("Identifying bad triangles");
                AddTriangleLinesToLayer(badTrianglesLayer);

                foreach (var t in Triangles)
                {
                    if(t.CircumcircleContainsVertex(p))
                    {
                        t.IsBad = true;
                        polygon.Add(new DelaunayEdge(t.V1, t.V2));
                        polygon.Add(new DelaunayEdge(t.V2, t.V3));
                        polygon.Add(new DelaunayEdge(t.V3, t.V1));

                        var hip = new HighlightPointsCommand
                        {
                            AssociatedAlgorithm = this,
                            HighlightLevel = 1
                        };

                        hip.Points.Add(new Vector { X = t.V1.X, Y = t.V1.Y });
                        hip.Points.Add(new Vector { X = t.V2.X, Y = t.V2.Y });
                        hip.Points.Add(new Vector { X = t.V3.X, Y = t.V3.Y });
                        badTrianglesLayer.AddCommand(hip);
                    }
                }

                RemoveBadTriangles();

                for (int c = 0; c < polygon.Count; c++)
                {
                    for(int d = c + 1; d < polygon.Count; d++)
                    {
                        if(polygon[c].AlmostEquals(polygon[d]))
                        {
                            polygon[c].BadEdge = true;
                            polygon[d].BadEdge = true;
                        }
                    }
                }

                RemoveBadEdges(polygon);

                foreach (var poly in polygon)
                {
                    Triangles.Add(new DelaunayTriangle { V1 = poly.VStart, V2 = poly.VEnd, V3 = p });
                }

                var triLayer = History.CreateAndAddNewLayer("for loop iteration i=" + i);
                AddTriangleLinesToLayer(triLayer);
            }

            RemoveTrianglesWithVertex(p1, p2, p3);

            foreach(var t in Triangles)
            {
                Edges.Add(new DelaunayEdge { VStart = t.V1, VEnd = t.V2 });
                Edges.Add(new DelaunayEdge { VStart = t.V2, VEnd = t.V3 });
                Edges.Add(new DelaunayEdge { VStart = t.V3, VEnd = t.V1 });
            }

            var layer = History.CreateAndAddNewLayer("Final Result");

            AddEdgesToLayer(layer);
        }
    }
}
