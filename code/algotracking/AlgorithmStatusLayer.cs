using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class AlgorithmStatusLayer
    {
        public List<AlgorithmStateCommand> Commands { get; set; }
        public string Comments { get; set; }

        public AlgorithmStatusLayer()
        {
            Commands = new List<AlgorithmStateCommand>();
        }

        public void AddCommand(AlgorithmStateCommand c)
        {
            Commands.Add(c);
        }
    }
}
