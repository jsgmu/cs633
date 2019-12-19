using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.fortune
{
    public class RBTree
    {
        //private TreeNode root;
        public TreeNode root { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }

        public RBTree()
        {
            Clear();
        }
        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public TreeNode GetFirst()
        {
            return GetFirst(root);
        }

        private TreeNode GetFirst(TreeNode node)
        {
            while(node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }

        private TreeNode GetLast(TreeNode node)
        {
            while(node.Right != null)
            {
                node = node.Right;
            }

            return node;
        }

        private int dumpCount = 0;
        public void DumpTree(string label)
        {
            Console.WriteLine("----- " + Name + " TREE DUMP " + dumpCount + ", " + label + " -------------");
            Console.WriteLine(root == null ? "null root" : root.ToString());
            Console.WriteLine();

            dumpCount++;
        }

        public void Dump(string label, int counter)
        {
            //console.log('tree: ' + label + ', counter: ' + counter);
            Console.WriteLine("tree: " + label + ", counter: " + counter);

            if (root == null)
                Console.WriteLine("null root");
            else
            {
                Console.WriteLine("tree dump, label: " + label + ", counter: " + counter);
                Console.WriteLine(root.ToString());
            }
            //if (!t || !t.root)
            //{
            //    console.log('   null root');
            //}
            //else
            //{
            //    console.log('tree dump, label: ' + label + ', counter: ' + counter, dumpTreeNode(t.root, 0, false, label));
            //}
        }
        private void RotateLeft(TreeNode node)
        {
            //var p = node,
            //    q = node.rbRight, // can't be null
            //    parent = p.rbParent;
            //if (parent)
            //{
            //    if (parent.rbLeft === p)
            //    {
            //        parent.rbLeft = q;
            //    }
            //    else
            //    {
            //        parent.rbRight = q;
            //    }
            //}
            //else
            //{
            //    this.root = q;
            //}
            //q.rbParent = parent;
            //p.rbParent = q;
            //p.rbRight = q.rbLeft;
            //if (p.rbRight)
            //{
            //    p.rbRight.rbParent = p;
            //}
            //q.rbLeft = p;

            TreeNode p = node, q = node.Right, parent = p.Parent;

            if(parent != null)
            {
                if(parent.Left == p)
                {
                    parent.Left = q;
                } else
                {
                    parent.Right = q;
                }
            }
            else
            {
                root = q;
            }

            q.Parent = parent;
            p.Parent = q;
            p.Right = q.Left;

            if(p.Right != null)
            {
                p.Right.Parent = p;
            }

            q.Left = p;
        }

        public void RotateRight(TreeNode node)
        {
            //var p = node,
            //    q = node.rbLeft, // can't be null
            //    parent = p.rbParent;
            //if (parent)
            //{
            //    if (parent.rbLeft === p)
            //    {
            //        parent.rbLeft = q;
            //    }
            //    else
            //    {
            //        parent.rbRight = q;
            //    }
            //}
            //else
            //{
            //    this.root = q;
            //}
            //q.rbParent = parent;
            //p.rbParent = q;
            //p.rbLeft = q.rbRight;
            //if (p.rbLeft)
            //{
            //    p.rbLeft.rbParent = p;
            //}
            //q.rbRight = p;

            TreeNode p = node, q = node.Left, parent = p.Parent;

            if(parent != null)
            {
                if(parent.Left == p)
                {
                    parent.Left = q;
                } else
                {
                    parent.Right = q;
                }
            } else
            {
                root = q;
            }

            q.Parent = parent;
            p.Parent = q;
            p.Left = q.Right;

            if(p.Left != null)
            {
                p.Left.Parent = p;
            }

            q.Right = p;
        }

        public void RemoveNode(TreeNode node)
        {
            //if (node.rbNext)
            //{
            //    node.rbNext.rbPrevious = node.rbPrevious;
            //}
            //if (node.rbPrevious)
            //{
            //    node.rbPrevious.rbNext = node.rbNext;
            //}
            //node.rbNext = node.rbPrevious = null;

            if(node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }

            if(node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }

            node.Next = node.Previous = null;

            //// <<<
            //var parent = node.rbParent,
            //    left = node.rbLeft,
            //    right = node.rbRight,
            //    next;

            TreeNode parent = node.Parent;
            TreeNode left = node.Left;
            TreeNode right = node.Right;
            TreeNode next = null;

            //if (!left)
            //{
            //    next = right;
            //}
            //else if (!right)
            //{
            //    next = left;
            //}
            //else
            //{
            //    next = this.getFirst(right);
            //}

            if(left == null)
            {
                next = right;
            } else if(right == null)
            {
                next = left;
            } else
            {
                next = GetFirst(right);
            }

            //if (parent)
            //{
            //    if (parent.rbLeft === node)
            //    {
            //        parent.rbLeft = next;
            //    }
            //    else
            //    {
            //        parent.rbRight = next;
            //    }
            //}
            //else
            //{
            //    this.root = next;
            //}

            if(parent != null)
            {
                if(parent.Left == node)
                {
                    parent.Left = next;
                } else
                {
                    parent.Right = next;
                }
            }
            else
            {
                root = next;
            }

            //// enforce red-black rules
            //var isRed;
            //if (left && right)
            //{
            //    isRed = next.rbRed;
            //    next.rbRed = node.rbRed;
            //    next.rbLeft = left;
            //    left.rbParent = next;
            //    if (next !== right)
            //    {
            //        parent = next.rbParent;
            //        next.rbParent = node.rbParent;
            //        node = next.rbRight;
            //        parent.rbLeft = node;
            //        next.rbRight = right;
            //        right.rbParent = next;
            //    }
            //    else
            //    {
            //        next.rbParent = parent;
            //        parent = next;
            //        node = next.rbRight;
            //    }
            //}
            //else
            //{
            //    isRed = node.rbRed;
            //    node = next;
            //}

            bool isRed = false;

            if(left != null && right != null)
            {
                isRed = next.IsRed();
                next.Color = node.Color;
                next.Left = left;
                left.Parent = next;

                if(next != right)
                {
                    parent = next.Parent;
                    next.Parent = node.Parent;
                    node = next.Right;
                    parent.Left = node;
                    next.Right = right;
                    right.Parent = next;
                } else
                {
                    next.Parent = parent;
                    parent = next;
                    node = next.Right;
                }
            } else
            {
                isRed = node.IsRed();
                node = next;
            }

            //// 'node' is now the sole successor's child and 'parent' its
            //// new parent (since the successor can have been moved)
            //if (node)
            //{
            //    node.rbParent = parent;
            //}
            //// the 'easy' cases
            //if (isRed) { return; }
            //if (node && node.rbRed)
            //{
            //    node.rbRed = false;
            //    return;
            //}

            if(node != null)
            {
                node.Parent = parent;
            }

            if(isRed)
            {
                return;
            }

            if(node != null && node.IsRed())
            {
                node.SetBlack();
                return;
            }

            //// the other cases
            //var sibling;
            TreeNode sibling = null;

            do
            {
                //    if (node === this.root)
                //    {
                //        break;
                //    }

                if(node == root)
                {
                    break;
                }

                //    if (node === parent.rbLeft)
                //    {
                //        sibling = parent.rbRight;
                //        if (sibling.rbRed)
                //        {
                //            sibling.rbRed = false;
                //            parent.rbRed = true;
                //            this.rbRotateLeft(parent);
                //            sibling = parent.rbRight;
                //        }
                //        if ((sibling.rbLeft && sibling.rbLeft.rbRed) || (sibling.rbRight && sibling.rbRight.rbRed))
                //        {
                //            if (!sibling.rbRight || !sibling.rbRight.rbRed)
                //            {
                //                sibling.rbLeft.rbRed = false;
                //                sibling.rbRed = true;
                //                this.rbRotateRight(sibling);
                //                sibling = parent.rbRight;
                //            }
                //            sibling.rbRed = parent.rbRed;
                //            parent.rbRed = sibling.rbRight.rbRed = false;
                //            this.rbRotateLeft(parent);
                //            node = this.root;
                //            break;
                //        }
                //    }

                if (node == parent.Left)
                {
                    sibling = parent.Right;

                    if(sibling.IsRed())
                    {
                        sibling.SetBlack();
                        parent.SetRed();
                        RotateLeft(parent);
                        sibling = parent.Right;
                    }

                    if((sibling.Left != null && sibling.Left.IsRed()) || (sibling.Right != null && sibling.Right.IsRed()))
                    {
                        if(sibling.Right == null || !sibling.Right.IsRed())
                        {
                            sibling.Left.SetBlack();
                            sibling.SetRed();
                            RotateRight(sibling);
                            sibling = parent.Right;
                        }

                        sibling.Color = parent.Color;
                        parent.SetBlack();
                        sibling.Right.SetBlack();
                        RotateLeft(parent);
                        node = root;
                        break;
                    }
                }
                else
                {
                    //    else
                    //    {
                    //        sibling = parent.rbLeft;
                    //        if (sibling.rbRed)
                    //        {
                    //            sibling.rbRed = false;
                    //            parent.rbRed = true;
                    //            this.rbRotateRight(parent);
                    //            sibling = parent.rbLeft;
                    //        }
                    //        if ((sibling.rbLeft && sibling.rbLeft.rbRed) || (sibling.rbRight && sibling.rbRight.rbRed))
                    //        {
                    //            if (!sibling.rbLeft || !sibling.rbLeft.rbRed)
                    //            {
                    //                sibling.rbRight.rbRed = false;
                    //                sibling.rbRed = true;
                    //                this.rbRotateLeft(sibling);
                    //                sibling = parent.rbLeft;
                    //            }
                    //            sibling.rbRed = parent.rbRed;
                    //            parent.rbRed = sibling.rbLeft.rbRed = false;
                    //            this.rbRotateRight(parent);
                    //            node = this.root;
                    //            break;
                    //        }
                    //    }

                    sibling = parent.Left;
                    if(sibling.IsRed())
                    {
                        sibling.SetBlack();
                        parent.SetRed();
                        RotateRight(parent);
                        sibling = parent.Left;
                    }

                    if( (sibling.Left != null && sibling.Left.IsRed()) || (sibling.Right != null && sibling.Right.IsRed()))
                    {
                        if(sibling.Left == null || !sibling.Left.IsRed())
                        {
                            sibling.Right.SetBlack();
                            sibling.SetRed();
                            RotateLeft(sibling);
                            sibling = parent.Left;
                        }

                        sibling.Color = parent.Color;
                        parent.SetBlack();
                        sibling.Left.SetBlack();
                        RotateRight(parent);
                        node = root;
                        break;
                    }
                }

                //    sibling.rbRed = true;
                //    node = parent;
                //    parent = parent.rbParent;

                sibling.SetRed();
                node = parent;
                parent = parent.Parent;
            //} while (node != null && !node.IsRed());
            } while (!node.IsRed());

            if (node != null)
            {
                node.SetBlack();
            }

            Count--;

            //DumpTree("after remove");

            //} while (!node.rbRed);
            //if (node) { node.rbRed = false; }

            //do
            //{
            //} while (!node.rbRed);
            //if (node) { node.rbRed = false; }
        }

        public void InsertSuccessor(TreeNode node, TreeNode successor)
        {
            //Console.WriteLine("****** INSERT: " + node.);
            //Console.WriteLine("node: " + node);
            //Console.WriteLine("successor: " + successor);

            TreeNode parent = null;

            //// start
            //var parent;
            //if (node)
            //{
            //}
            if (node != null)
            {
                //    // >>> rhill 2011-05-27: Performance: cache previous/next nodes
                //    successor.rbPrevious = node;
                //    successor.rbNext = node.rbNext;
                successor.Previous = node;
                successor.Next = node.Next;

                //    if (node.rbNext)
                //    {
                //        node.rbNext.rbPrevious = successor;
                //    }
                if (node.Next != null)
                {
                    node.Next.Previous = successor;
                }

                //    node.rbNext = successor;
                node.Next = successor;

                //    // <<<
                //    if (node.rbRight)
                //    {
                //        // in-place expansion of node.rbRight.getFirst();
                //        node = node.rbRight;
                //        while (node.rbLeft) { node = node.rbLeft; }
                //        node.rbLeft = successor;
                //    }
                //    else
                //    {
                //        node.rbRight = successor;
                //    }
                //    parent = node;
                if (node.Right != null)
                {
                    node = node.Right;

                    while(node.Left != null)
                    {
                        node = node.Left;
                    }

                    node.Left = successor;
                } else
                {
                    node.Right = successor;
                }

                parent = node;
            }

            //// rhill 2011-06-07: if node is null, successor must be inserted
            //// to the left-most part of the tree
            //else if (this.root)
            //{
            //}
            //else
            //{
            //}

            else if(root != null)
            {
                //    node = this.getFirst(this.root);
                //    // >>> Performance: cache previous/next nodes
                //    successor.rbPrevious = null;
                //    successor.rbNext = node;
                //    node.rbPrevious = successor;
                //    // <<<
                //    node.rbLeft = successor;
                //    parent = node;

                node = GetFirst(root);
                successor.Previous = null;
                successor.Next = node;
                node.Previous = successor;
                node.Left = successor;
                parent = node;
            }
            else
            {
                //    // >>> Performance: cache previous/next nodes
                //    successor.rbPrevious = successor.rbNext = null;
                //    // <<<
                //    this.root = successor;
                //    parent = null;

                successor.Previous = null;
                successor.Next = null;
                root = successor;
                parent = null;
            }

            //successor.rbLeft = successor.rbRight = null;
            //successor.rbParent = parent;
            //successor.rbRed = true;

            successor.Left = null;
            successor.Right = null;
            successor.Parent = parent;
            successor.SetRed();

            //// Fixup the modified tree by recoloring nodes and performing
            //// rotations (2 at most) hence the red-black tree properties are
            //// preserved.
            //var grandpa, uncle;
            //node = successor;

            TreeNode grandpa, uncle;
            node = successor;

            //while (parent && parent.rbRed)
            //{
            while(parent != null && parent.IsRed())
            {
                //    grandpa = parent.rbParent;
                grandpa = parent.Parent;

                //    if (parent === grandpa.rbLeft)
                //    {
                if (parent == grandpa.Left)
                {
                    //        uncle = grandpa.rbRight;
                    //        if (uncle && uncle.rbRed)
                    //        {
                    //            parent.rbRed = uncle.rbRed = false;
                    //            grandpa.rbRed = true;
                    //            node = grandpa;
                    //        }
                    //        else
                    //        {
                    //            if (node === parent.rbRight)
                    //            {
                    //                this.rbRotateLeft(parent);
                    //                node = parent;
                    //                parent = node.rbParent;
                    //            }
                    //            parent.rbRed = false;
                    //            grandpa.rbRed = true;
                    //            this.rbRotateRight(grandpa);
                    //        }
                    //    }

                    uncle = grandpa.Right;
                    if(uncle != null && uncle.IsRed())
                    {
                        parent.SetBlack();
                        uncle.SetBlack();
                        grandpa.SetRed();
                        node = grandpa;
                    } else
                    {
                        if(node == parent.Right)
                        {
                            RotateLeft(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        parent.SetBlack();
                        grandpa.SetRed();
                        RotateRight(grandpa);
                    }
                }
                else {
                    //    else
                    //    {
                    //        uncle = grandpa.rbLeft;
                    //        if (uncle && uncle.rbRed)
                    //        {
                    //            parent.rbRed = uncle.rbRed = false;
                    //            grandpa.rbRed = true;
                    //            node = grandpa;
                    //        }
                    //        else
                    //        {
                    //            if (node === parent.rbLeft)
                    //            {
                    //                this.rbRotateRight(parent);
                    //                node = parent;
                    //                parent = node.rbParent;
                    //            }
                    //            parent.rbRed = false;
                    //            grandpa.rbRed = true;
                    //            this.rbRotateLeft(grandpa);
                    //        }
                    //    }
                    //    parent = node.rbParent;
                    //}

                    uncle = grandpa.Left;

                    if(uncle != null && uncle.IsRed())
                    {
                        parent.SetBlack();
                        uncle.SetBlack();
                        grandpa.SetRed();
                        node = grandpa;
                    } else
                    {
                        if(node == parent.Left)
                        {
                            RotateRight(parent);
                            node = parent;
                            parent = node.Parent;
                        }

                        parent.SetBlack();
                        grandpa.SetRed();
                        RotateLeft(grandpa);
                    }
                }

                parent = node.Parent;
            }

            //this.root.rbRed = false;
            root.SetBlack();
            // end

            //DumpTree("after insert");

            Count++;
        }
    }
}
