using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.basics
{
    public class DotProduct : AlgorithmBase
    {
        public double Result { get; set; }

        private double ComputeDotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public override void Run()
        {
            var v1 = InputPoints[0];
            var v2 = InputPoints[1];

            Result = ComputeDotProduct(v1, v2);
        }
    }
}
