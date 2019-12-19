using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class TreeNode : IComparable<TreeNode>
    {
        public object Data { get; set; } // only used in former implementation

        public Site Site { get; set; }
        public BeachSection BeachSection { get; set; }
        public CircleEvent CircleEvent { get; set; }
        public TreeNode CircleEventNode { get; set; }

        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode Parent { get; set; }
        public TreeNode Next { get; set; }
        public TreeNode Previous { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Color { get; set; } // 0 = red, 1 = black

        public TreeNode()
        {
            Color = 1;
        }

        public string GetFullString(int level, bool doNotFollow = false)
        {
            String str = "";

            for (int i = 0; i < 3 * level; i++)
            {
                str = str + " ";
            }

            var nodeShort = "(x=" + X + ", y=" + Y + "), color=" + (Color == 0 ? "Red" : "Black") + ", details: ";

            var siteStr = (Site == null) ? "null" : Site.ToString();
            var beachStr = (BeachSection == null) ? "null" : BeachSection.ToString();
            var ceStr = (CircleEvent == null) ? "null" : CircleEvent.ToString();

            return str + nodeShort + "[site=" + siteStr + ", beach=" + beachStr + ", circleEvent=" + ceStr + "]\n"
                + str + ("(" + level + ") ") + "LEFT: " + NodeString(Left, level+1, doNotFollow) + "\n"
                + str + ("(" + level + ") ") + "RIGHT: " + NodeString(Right, level + 1, doNotFollow) + "\n"
                + str + ("(" + level + ") ") + "PREVIOUS: " + NodeString(Previous, level, true) + "\n"
                + str + ("(" + level + ") ") + "NEXT: " + NodeString(Next, level, true) + "\n";
        }

        private string GetReallyShortString()
        {
            var nodeShort = "(x=" + X + ", y=" + Y + "), color=" + (Color == 0 ? "Red" : "Black") + ", details: ";

            var siteStr = (Site == null) ? "null" : Site.ToString();
            var beachStr = (BeachSection == null) ? "null" : BeachSection.ToString();
            var ceStr = (CircleEvent == null) ? "null" : CircleEvent.ToString();

            return nodeShort + "[site=" + siteStr + ", beach=" + beachStr + ", circleEvent=" + ceStr + "]";
        }

        private string NodeStringShort(TreeNode n, int level, bool doNotFollow = false)
        {
            if (n == null)
                return "null node";

            return n.GetReallyShortString();
        }

        private string NodeString(TreeNode n, int level, bool doNotFollow = false)
        {
            String str = "";

            for(int i = 0; i < 3*level; i++)
            {
                str = str + " ";
            }

            if (n == null)
                return "null";

            if (doNotFollow)
            {
                // return "not null, not following. " + n.GetReallyShortString();

                //return str + nodeShort + "[site=" + siteStr + ", beach=" + beachStr + ", circleEvent=" + ceStr + "]\n"
                //    + str + ("(" + level + ") ") + "LEFT: " + NodeString(Left, level + 1, doNotFollow) + "\n"
                //    + str + ("(" + level + ") ") + "RIGHT: " + NodeString(Right, level + 1, doNotFollow) + "\n"
                //    + str + ("(" + level + ") ") + "PREVIOUS: " + NodeString(Previous, level, true) + "\n"
                //    + str + ("(" + level + ") ") + "NEXT: " + NodeString(Next, level, true) + "\n";

                return str + GetReallyShortString()
                    + str + " do not follow is on; \n"
                    + str + str + ("(" + (level+1) + ") ") + "LEFT: " + NodeStringShort(Left, level + 1, doNotFollow) + "\n"
                    + str + str + ("(" + (level + 1) + ") ") + "RIGHT: " + NodeStringShort(Right, level + 1, doNotFollow) + "\n"
                    + str + str + ("(" + (level + 1) + ") ") + "PREVIOUS: " + NodeStringShort(Previous, level + 1, doNotFollow) + "\n"
                    + str + str + ("(" + (level + 1) + ") ") + "NEXT: " + NodeStringShort(Next, level + 1, doNotFollow) + "\n";
            }

            return n.GetFullString(level, doNotFollow);
        }

        public override string ToString()
        {
            //return "|| (x=" + X + ", y=" + Y + "), color=" + (Color == 0 ? "Red" : "Black") + ", details: " + GetFullString() + " ||\n" +;
            return GetFullString(0);
        }

        //public CircleEvent AsCircleEvent()
        //{
        //    if (!(Data is CircleEvent))
        //        return null;

        //    return (CircleEvent)Data;
        //}

        //public BeachSection AsBeachSection()
        //{
        //    if (!(Data is BeachSection))
        //        return null;

        //    return (BeachSection)Data;
        //}

        public int CompareTo(TreeNode other)
        {
            if (Y < other.Y)
                return -1;

            if (Y == other.Y && X <= other.X)
                return -1;

            return 1;
        }

        public bool IsRed()
        {
            return Color == 0;
        }

        public bool IsBlack()
        {
            return Color == 1;
        }

        public void SetRed()
        {
            Color = 0;
        }

        public void SetBlack()
        {
            Color = 1;
        }

        public TreeNode Sibling()
        {
            if (Parent == null)
                return null;

            return Parent.Left == this ? Parent.Right : Parent.Left;
        }

    }
}
