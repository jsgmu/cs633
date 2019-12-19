using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CompGeomVis
{
    /// <summary>
    /// Interaction logic for TreeDrawControl.xaml
    /// </summary>
    public partial class TreeDrawControl : UserControl
    {
        private int[] treeData;

        public TreeDrawControl()
        {
            InitializeComponent();

            // left: index * 2 + 1
            // right: index * 2 + 2
            treeData = new int[7];

            treeData[0] = 100; // root
            treeData[1] = 50; // 0 * 2 + 1 = 1
            treeData[2] = 150; // 0 * 2 + 2 = 2
            treeData[3] = 25; // 1 * 2 + 1 = 3
            treeData[4] = 75; // 1 * 2 + 2 = 4
            treeData[5] = 125; // 2 * 2 + 1 = 5
            treeData[6] = 175; // 2 * 2 + 2 = 6
        }

        private int GetLeft(int nodeIndex)
        {
            return treeData[nodeIndex * 2 + 1];
        }

        private int GetRight(int nodeIndex)
        {
            return treeData[nodeIndex * 2 + 2];
        }

        private bool HasLeft(int nodeIndex)
        {
            return (nodeIndex * 2 + 1) < treeData.Length;
        }

        private bool HasRight(int nodeIndex)
        {
            return (nodeIndex * 2 + 1) < treeData.Length;
        }

        private int CountLeafs()
        {
            return 4;
        }
    }
}
