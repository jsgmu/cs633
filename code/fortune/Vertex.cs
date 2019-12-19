using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class Vertex
    {
        public int Id { get; set; }
        private static int NextId = 0;

        public double X { get; set; }
        public double Y { get; set; }

        public Vertex()
        {
            Id = NextId;
            NextId++;
        }

        public Vertex(double x, double y) : this()
        {
            X = x;
            Y = y;
        }
    }
}
