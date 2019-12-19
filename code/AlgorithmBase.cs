using CompGeomVis.algotracking;
using CompGeomVis.canvas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public abstract class AlgorithmBase
    {
        public int AlgorithmId { get; set; }

        protected List<Vector> InputPoints;
        protected List<LineModel> InputLines;
        protected List<PolygonModel> InputPolygons;
        public AlgorithmHistory History { get; set; }

        public void SetInputPoints(List<Vector> points)
        {
            InputPoints = points;
        }

        public void SetInputPolygons(List<PolygonModel> polys)
        {
            InputPolygons = polys;
        }

        protected void AddLineCommand(AlgorithmStatusLayer layer, Vector v1, Vector v2)
        {
            if (v1.Alternates == null || v2.Alternates == null)
            {
                //Debugger.Break();
                return;
            }

            layer.AddCommand(new AddLineCommand
            {
                AssociatedAlgorithm = this,
                StartXIndex = v1.Alternates.DotIndexLeft,
                StartYIndex = v1.Alternates.DotIndexTop,
                EndXIndex = v2.Alternates.DotIndexLeft,
                EndYIndex = v2.Alternates.DotIndexTop,
                Comments = ""
            });
        }

        protected void AddNonIndexedLineCommand(AlgorithmStatusLayer layer, Vector v1, Vector v2)
        {
            layer.AddCommand(new AddNonIndexedLineCommand
            {
                AssociatedAlgorithm = this,
                StartX = v1.X,
                StartY = v1.Y,
                EndX = v2.X,
                EndY = v2.Y,
                Comments = ""
            });
        }

        protected void AddNonIndexedPolygonCommand(AlgorithmStatusLayer layer, PolygonModel poly)
        {
            layer.AddCommand(new AddNonIndexedPolygonCommand
            {
                AssociatedAlgorithm = this,
                Polygon = poly
            });
        }

        public abstract void Run();
    }
}
