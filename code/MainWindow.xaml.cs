using CompGeomVis.events;
using CompGeomVis.models;
using CompGeomVis.polygonintersection;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHandle<DemoCreated>, IHandle<HighlightLiveCode>,
        IHandle<ClearTextStatus>, IHandle<AddTextStatus>
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowHelper.MainWindow = this;

            this.KeyUp += MainWindow_KeyUp;

            Data.Load();

            descriptionTextBox.Text = ""; // Data.Algorithms[0].Description;
            liveCodeControl.Text = ""; // Data.Algorithms[0].LiveCodeTextText;

            pseudoCodeControl.StyleResetDefault();
            pseudoCodeControl.Styles[ScintillaNET.Style.Default].Font = "Consolas";
            pseudoCodeControl.Styles[ScintillaNET.Style.Default].Size = 12;
            pseudoCodeControl.Styles[ScintillaNET.Style.Default].BackColor = IntToColor(0x000080);
            pseudoCodeControl.Styles[ScintillaNET.Style.Default].ForeColor = IntToColor(0xFFFF00);
            pseudoCodeControl.StyleClearAll();
            pseudoCodeControl.Styles[ScintillaNET.Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            pseudoCodeControl.Styles[ScintillaNET.Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            pseudoCodeControl.Styles[ScintillaNET.Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            pseudoCodeControl.Styles[ScintillaNET.Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = pseudoCodeControl.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            liveCodeControl.StyleResetDefault();
            liveCodeControl.Styles[ScintillaNET.Style.Default].Font = "Consolas";
            liveCodeControl.Styles[ScintillaNET.Style.Default].Size = 14;
            liveCodeControl.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Default].ForeColor = System.Drawing.Color.Silver;
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Comment].ForeColor = System.Drawing.Color.FromArgb(0, 128, 0); // Green
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.CommentLine].ForeColor = System.Drawing.Color.FromArgb(0, 128, 0); // Green
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.CommentLineDoc].ForeColor = System.Drawing.Color.FromArgb(128, 128, 128); // Gray
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Number].ForeColor = System.Drawing.Color.Olive;
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Word].ForeColor = System.Drawing.Color.Blue;
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Word2].ForeColor = System.Drawing.Color.Blue;
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.String].ForeColor = System.Drawing.Color.FromArgb(163, 21, 21); // Red
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Character].ForeColor = System.Drawing.Color.FromArgb(163, 21, 21); // Red
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Verbatim].ForeColor = System.Drawing.Color.FromArgb(163, 21, 21); // Red
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.StringEol].BackColor = System.Drawing.Color.Pink;
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Operator].ForeColor = System.Drawing.Color.Purple;
            liveCodeControl.Styles[ScintillaNET.Style.Cpp.Preprocessor].ForeColor = System.Drawing.Color.Maroon;
            liveCodeControl.Lexer = Lexer.Cpp;

            // Set the keywords
            liveCodeControl.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            liveCodeControl.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");

            nums = liveCodeControl.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            Data.LoadWorkspace();

            EventBus.Subscribe(this);

            treeItemRoots = new TreeViewItem[] { algorithm0, algorithm1, algorithm2, algorithm3,
                                                 algorithm4, algorithm5, algorithm6 };

            LoadWorkspace();
        }

        private TreeViewItem FindTreeViewItemByAlgId(int id)
        {
            foreach(var tvi in treeItemRoots)
            {
                if (Int32.Parse((string)tvi.Tag) == id)
                    return tvi;
            }

            return null;
        }

        private void LoadWorkspace()
        {
            if (Data.WorkSpace == null)
                return;

            foreach(var d in Data.WorkSpace)
            {
                var tvi = new TreeViewItem();
                tvi.Header = d.ShortTitle;
                tvi.Tag = d;
                tvi.MouseDoubleClick += Tvi_MouseDoubleClick;
                var root = FindTreeViewItemByAlgId(d.AlgorithmId);
                if(root != null)
                    root.Items.Add(tvi);
            }
        }

        private void LiveCodeControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        private const int BACK_COLOR = 0x2A211C;
        private const int FORE_COLOR = 0xB7B7B7;
        private const int NUMBER_MARGIN = 1;

        public static System.Drawing.Color IntToColor(int rgb)
        {
            return System.Drawing.Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private void CanvasTools_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;

            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void SaveCanvas()
        {
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.L)
            {
                //activeMode = 1;
                //RefreshStatus();
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //DrawGrid();
        }

        private void TestGrid_Click(object sender, RoutedEventArgs e)
        {
            //DrawGrid();
        }

        private void TestSave_Click(object sender, RoutedEventArgs e)
        {
            SaveCanvas();
        }

        private void FortuneButton_Click(object sender, RoutedEventArgs e)
        {
            //var f = new fortune.FortunesAlgorithm();
            //f.GenerateSites(10, 800, 600);

            var f2 = new fortune.FortuneWithTree();
            f2.GenerateSites(10, 800, 600);

            //f.GenerateSites(100, 800, 600);

            Console.WriteLine("# CELLS: " + f2.Cells.Count);
            Console.WriteLine("# EDGES: " + f2.Edges.Count);
            Console.WriteLine("# ARCS: " + f2.BeachLine.Count);
        }

        private void DemoAreaTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private int GetTreeItemLevel(TreeViewItem item)
        {
            if (item == null)
                return 0;

            return 1 + GetTreeItemLevel(item.Parent as TreeViewItem);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void CreateDemoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowHelper.CreateNewDemoWindow();
        }

        private TreeViewItem[] treeItemRoots;

        public void Handle(DemoCreated eventData)
        {
            var tvi = new TreeViewItem();
            tvi.Header = eventData.Demo.ShortTitle;
            tvi.Tag = eventData.Demo;
            tvi.MouseDoubleClick += Tvi_MouseDoubleClick;

            var root = FindTreeViewItemByAlgId(eventData.Demo.AlgorithmId);

            if (root != null)
            {
                root.Items.Add(tvi);
                root.IsExpanded = true;
            }

            Data.WorkSpace.Add(eventData.Demo);

            OpenDemo(eventData.Demo);
        }

        private void Tvi_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var demo = (DemoModel)((TreeViewItem)sender).Tag;

            OpenDemo(demo);
        }

        private void OpenDemo(DemoModel demo)
        {
            bool alreadyOpen = false;
            int index = 0;

            foreach(var t in DemoAreaTabs.Items)
            {
                if(((TabItem)t).Tag == demo)
                {
                    alreadyOpen = true;
                    break;
                }

                index++;
            }

            if (alreadyOpen)
            {
                DemoAreaTabs.SelectedIndex = index;
                var t = DemoAreaTabs.Items[index] as CloseableTab;
                if (t == null)
                    return;
                var dac = t.Content as DemoAreaControl;
                if (dac == null)
                    return;
                dac.Reload();
                return;
            }

            var d = new DemoAreaControl();
            d.LoadDemo(demo);
            var tab = new CloseableTab { Title = demo.ShortTitle };
            tab.Content = d;
            tab.Tag = demo;
            DemoAreaTabs.Items.Add(tab);
            tab.Focus();

            var alg = Data.FindAlgoById(demo.AlgorithmId);

            if (alg != null)
            {
                liveCodeControl.Text = string.Join("\n", alg.LiveCodeTextText);
                pseudoCodeControl.Text = string.Join("\n", alg.PseudoCodeText);
                descriptionTextBox.Text = alg.Description;
                degenerateCasesTextBox.Text = alg.DegenerateCaseText;
            }
        }

        private void PolyIntersectionTest_Click(object sender, RoutedEventArgs e)
        {
            var p = new ConvexPolyIntersection();

            //p.Test();
        }

        public void Handle(HighlightLiveCode e)
        {
            var alg = Data.FindAlgoById(e.AlgorithmId);

            //         public Dictionary<string, List<int>> LiveCodeSections { get; set; }
            bool first = true;

            foreach (var s in e.Sections)
            {
                foreach(var lineIndex in alg.LiveCodeSections[s])
                {
                    if(first)
                    {
                        liveCodeControl.SetSelection(liveCodeControl.Lines[lineIndex].Position, liveCodeControl.Lines[lineIndex].EndPosition);
                        first = false;
                    } else
                    {
                        liveCodeControl.AddSelection(liveCodeControl.Lines[lineIndex].Position, liveCodeControl.Lines[lineIndex].EndPosition);
                    }
                }
            }
        }

        private void LoadDemoTest_Click(object sender, RoutedEventArgs e)
        {
            //var d = new DemoModel();

            //BoundingBoxAlgorithm b = new BoundingBoxAlgorithm();

            //b.Run();
        }

        private void SaveWorkspace_Click(object sender, RoutedEventArgs e)
        {
            Data.SaveWorkspace();
            MessageBox.Show("Workspace (demos) saved successfully.", "Success");
        }

        private void CreateDemoButton_Click(object sender, RoutedEventArgs e)
        {
            WindowHelper.CreateNewDemoWindow();
        }

        public void Handle(AddTextStatus e)
        {
            commentaryTextBox.Text += e.Text;
            commentaryTextBox.Text += "\r\n";
        }

        public void Handle(ClearTextStatus eventData)
        {
            commentaryTextBox.Text = "";
        }
    }
}
