using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class Site : IComparable<Site>
    {
        public int Id { get; set; }
        public static int NextId = 1;

        public double X { get; set; }
        public double Y { get; set; }

        public Site()
        {
            Id = NextId;
            NextId++;
        }

        public Site(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "ID=" + Id + ", X=" + X + ", Y=" + Y;
        }

        public int CompareTo(Site other)
        {
            if (Y < other.Y)
                return -1;

            if (Y == other.Y && X <= other.X)
                return -1;

            return 1;
        }
    }
}
