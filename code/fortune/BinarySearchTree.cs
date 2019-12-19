using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class BinarySearchTree
    {
        private TreeNode Root;
        public string Name { get; set; }

        public BinarySearchTree()
        {
            Root = null;
        }

        public void Insert(object o)
        {
            var x = (o is Site) ? ((Site)o).X : ((CircleEvent)o).X;
            var y = (o is Site) ? ((Site)o).Y : ((CircleEvent)o).Y;
            TreeNode newNode = new TreeNode { Data = o, X = x, Y = y };

            Root = Insert(Root, newNode);
        }

        private TreeNode Insert(TreeNode node, TreeNode newNode)
        {
            if (node == null)
            {
                return newNode;
            }
            else
            {

                if (newNode.CompareTo(node) < 0)
                {
                    node.Left = Insert(node.Left, newNode);
                }
                else
                {
                    node.Right = Insert(node.Right, newNode);
                }
            }

            return newNode;
        }

        public void Remove(object o)
        {

        }
    }
}
