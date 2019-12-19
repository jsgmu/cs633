using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class AlgorithmHistoryItem
    {
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int MajorStep { get; set; }
        public int MinorStep { get; set; }
        public List<Vector> InputPoints { get; set; }
        public List<LineModel> InputLines { get; set; }
        public List<Vector> SortedInputPoints { get; set; }
        public List<LineModel> StatusLines { get; set; }
        public Stack<Vector> VectorStack { get; set; }
        public AlgorithmHistoryItem()
        {
            InputPoints = new List<Vector>();
            InputLines = new List<LineModel>();
            StatusLines = new List<LineModel>();
            SortedInputPoints = new List<Vector>();
        }

        private Vector Copy(Vector v)
        {
            Vector vNew = new Vector();

            vNew.X = v.X;
            vNew.Y = v.Y;
            vNew.Is2D = v.Is2D;
            vNew.Is3D = v.Is3D;

            return vNew;
        }

        private LineModel Copy(LineModel line)
        {
            LineModel newLine = new LineModel();

            newLine.StartPoint = Copy(line.StartPoint);
            newLine.EndPoint = Copy(line.EndPoint);

            return newLine;
        }

        private void CopyLines(List<LineModel> source, List<LineModel> dest)
        {
            dest.Clear();

            foreach(var line in source)
            {
                dest.Add(Copy(line));
            }
        }

        private void CopyPoints(List<Vector> source, List<Vector> dest)
        {
            dest.Clear();

            foreach(var v in source)
            {
                dest.Add(Copy(v));
            }
        }

        public void SaveInputPoints(List<Vector> inputPoints)
        {
            CopyPoints(inputPoints, InputPoints);
        }

        public void SaveSortedInputPoints(List<Vector> points)
        {
            CopyPoints(points, SortedInputPoints);
        }

        public void SaveVectorStack(Stack<Vector> orig)
        {
            VectorStack = new Stack<Vector>();

            for (int i = orig.Count - 1; i >= 0; i--)
            {
                VectorStack.Push(Copy(orig.ElementAt(i)));
            }
        }
    }
}
