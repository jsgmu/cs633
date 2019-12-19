using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.convexity
{
    // This convexity check was implemented but not tested and not added to the set of
    // algorithms displayed in the tool
    public class ConvexityCheck
    {
        public PolygonModel InputPolygon { get; set; }

        private Vector GetPoint(int index, int offset)
        {
            int i = (index + offset) % InputPolygon.Lines.Count;

            return InputPolygon.Lines[i].StartPoint;
        }

        public bool IsPolygonConvex()
        {
            if (InputPolygon == null || InputPolygon.Lines.Count < 3)
                return false;

            if (InputPolygon.Lines.Count == 3)
                return true;

            int lastSign = Math.Sign(0);
            bool hasLastSign = false;

            for(int i = 0; i < InputPolygon.Lines.Count; i++)
            {
                Vector v1 = GetPoint(i, 0);
                Vector v2 = GetPoint(i, 1);
                Vector v3 = GetPoint(i, 2);

                double cp = GeomMath.CrossProduct(v1, v2, v3);

                int sign = Math.Sign(cp);

                if (hasLastSign && sign != lastSign)
                    return false;

                hasLastSign = true;
                lastSign = sign;
            }

            return true;
        }
    }
}
