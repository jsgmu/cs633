using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.algotracking
{
    public class AlgorithmUtil
    {
        public static Stack<Vector> CopyVectorStack(Stack<Vector> incomingStack)
        {
            Stack<Vector> copiedStack = new Stack<Vector>();

            foreach(var item in incomingStack)
            {
                copiedStack.Push(new Vector { X = item.X, Y = item.Y });
            }

            return copiedStack;
        }

        public static List<Vector> CopyVectorList(List<Vector> incomingList)
        {
            List<Vector> copiedList = new List<Vector>();

            foreach(var item in incomingList)
            {
                copiedList.Add(new Vector { X = item.X, Y = item.Y });
            }

            return copiedList;
        }
    }
}
