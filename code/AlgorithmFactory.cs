using CompGeomVis.polygonintersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class AlgorithmFactory
    {
        public static AlgorithmBase CreateById(int id)
        {
            if(id == 10)
            {
                return new grahamscan.GrahamScanAlgorithm();
            }
            else if (id == 20)
                return new delaunay.BowyerWatsonAlgorithm();
            else if (id == 21)
                return new delaunator.Delaunator();
            else if (id == 50)
                return new ConvexPolyIntersection();

            return null;
        }
    }
}
