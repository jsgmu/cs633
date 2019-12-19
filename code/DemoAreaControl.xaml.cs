using CompGeomVis.algotracking;
using CompGeomVis.canvas;
using CompGeomVis.events;
using CompGeomVis.grahamscan;
using CompGeomVis.models;
using Microsoft.Win32;
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
using Point = System.Windows.Point;

namespace CompGeomVis
{
    /// <summary>
    /// Interaction logic for DemoAreaControl.xaml
    /// </summary>
    public partial class DemoAreaControl : UserControl
    {
        private const int TOOL_MODE_SELECTION = 0;
        private const int TOOL_MODE_POINT = 1;
        private const int TOOL_MODE_LINE = 2;
        private const int TOOL_MODE_POLYGON = 3;
        private const int TOOL_MODE_TEXT = 4;
        private string[] ToolNames = { "Selection", "Point", "Line", "Polygon", "Text" };

        private String lastStatus = "";
        private int point1_xi, point1_yi;
        private int point2_xi, point2_yi;
        private int pointsSelected = 0;
        private CanvasWrapper CanvasWrapper { get; set; }
        private int activeMode = 0;
        private AlgorithmBase associatedAlgorithm;
        private DemoModel demo;

        public DemoAreaControl()
        {
            InitializeComponent();

            drawCanvas.MouseMove += DrawCanvas_MouseMove;
            drawCanvas.MouseLeftButtonUp += DrawCanvas_MouseLeftButtonUp;

            CanvasWrapper = new CanvasWrapper(drawCanvas);

            associatedAlgorithm = null;

            demo = new DemoModel();
        }

        public void LoadDemo(DemoModel d)
        {
            demo = d;
            TitleText.Text = demo.Title;
            CanvasWrapper.LoadDemo(demo);

            if (demo.Points != null && demo.Points.Count > 0)
            {
                foreach (var v in demo.Points)
                {
                    EventBus.Publish(new InputPointSelected(v.Alternates.DotIndexLeft, v.Alternates.DotIndexTop, 
                                                                 v.X, v.Y));
                    CanvasWrapper.AddInputPoint(v.Alternates.DotIndexLeft, v.Alternates.DotIndexTop, v.X, v.Y);
                }
            }

            if(demo.Polygons != null && demo.Polygons.Count > 0)
            {
                foreach(var poly in demo.Polygons)
                {
                    foreach(var line in poly.Lines)
                    {
                        CanvasWrapper.AddPolygonLine(
                            line.StartPoint.Alternates.DotIndexLeft, line.StartPoint.Alternates.DotIndexTop,
                            line.EndPoint.Alternates.DotIndexLeft, line.EndPoint.Alternates.DotIndexTop,
                            line.StartPoint.X, line.StartPoint.Y,
                            line.EndPoint.X, line.EndPoint.Y
                        );
                    }
                }
            }
        }

        public void Reload()
        {

        }

        private void DrawCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void DrawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var p = CoordHelper.TranslateFromMouse(CanvasWrapper.Config, 
                                                   e.GetPosition(drawCanvas).X, 
                                                   e.GetPosition(drawCanvas).Y);

            var xps = string.Format("{0:0.###}", p.ValueX);
            var yps = string.Format("{0:0.###}", p.ValueY);

            UpdateStatus("POINT: (" + xps + ", " + yps + ")");
        }

        private void CompleteLine()
        {
            CanvasWrapper.AddInputLine(point1_xi, point1_yi, point2_xi, point2_yi);
            pointsSelected = 0;
        }

        private void AddPolygonLine()
        {
            var vx1 = CanvasWrapper.Config.ValueTickX * point1_xi + CanvasWrapper.Config.XMin;
            var vy1 = CanvasWrapper.Config.ValueTickY * (CanvasWrapper.Config.YCount - point1_yi) + CanvasWrapper.Config.YMin;

            var vx2 = CanvasWrapper.Config.ValueTickX * point2_xi + CanvasWrapper.Config.XMin;
            var vy2 = CanvasWrapper.Config.ValueTickY * (CanvasWrapper.Config.YCount - point2_yi) + CanvasWrapper.Config.YMin;

            CanvasWrapper.AddPolygonLine(point1_xi, point1_yi, point2_xi, point2_yi, vx1, vy1, vx2, vy2);

            if(CanvasWrapper.PolygonJustCompleted())
            {
                demo.Polygons.Add(new PolygonModel(CanvasWrapper.LastPolygon()));
                pointsSelected = 0;
            }
        }

        private void CancelActivePolygon()
        {
            CanvasWrapper.CancelActivePolygon();
            pointsSelected = 0;
        }

        private void UpdateStatus(string text)
        {
            string mode = ToolNames[activeMode];

            //CurrentCoordsTB.Text = "MODE: " + mode + ", " + text;
            CurrentCoordsTB.Text = "AXIS: " + CanvasWrapper.Config.FormatAxis() + ", MODE: " + mode + ", " + text;
            lastStatus = text;
        }

        private void RefreshStatus()
        {
            UpdateStatus(lastStatus);
        }

        private void AddInputPoint(int xi, int yi, double x, double y)
        {
            EventBus.Publish(new InputPointSelected(xi, yi, x, y));
            CanvasWrapper.AddInputPoint(xi, yi);
            demo.Points.Add(new Vector { X = x, Y = y,
                Alternates = new CanvasPoint { DotIndexLeft = xi, DotIndexTop = yi } });
        }

        private void DrawCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanvasPoint p = CoordHelper.TranslateFromMouse(CanvasWrapper.Config, e.GetPosition(drawCanvas).X, e.GetPosition(drawCanvas).Y);

            if (activeMode == TOOL_MODE_POINT)
            {
                EventBus.Publish(new InputPointSelected(p.DotIndexLeft, p.DotIndexTop, p.ValueX, p.ValueY));
                CanvasWrapper.AddInputPoint(p.DotIndexLeft, p.DotIndexTop, p.ValueX, p.ValueY);
                demo.Points.Add(new Vector { X = p.ValueX, Y = p.ValueY, Alternates = new CanvasPoint { DotIndexLeft = p.DotIndexLeft, DotIndexTop = p.DotIndexTop } });
                return;
            }

            if (activeMode == TOOL_MODE_LINE)
            {
                if (pointsSelected == 0)
                {
                    point1_xi = p.DotIndexLeft;
                    point1_yi = p.DotIndexTop;
                    pointsSelected++;
                }
                else if (pointsSelected == 1)
                {
                    pointsSelected++;
                    point2_xi = p.DotIndexLeft;
                    point2_yi = p.DotIndexTop;
                    CompleteLine();
                }
            }

            if(activeMode == TOOL_MODE_POLYGON)
            {
                if (pointsSelected == 0)
                {
                    point1_xi = p.DotIndexLeft;
                    point1_yi = p.DotIndexTop;
                    pointsSelected++;
                }
                else if (pointsSelected == 1)
                {
                    pointsSelected++;
                    point2_xi = p.DotIndexLeft;
                    point2_yi = p.DotIndexTop;
                    AddPolygonLine();
                } else if(pointsSelected >= 2)
                {
                    pointsSelected++;

                    // last end point becomes new start point
                    point1_xi = point2_xi;
                    point1_yi = point2_yi;
                    point2_xi = p.DotIndexLeft;
                    point2_yi = p.DotIndexTop;

                    AddPolygonLine();
                }
            }
        }

        private void TextTool_Click(object sender, RoutedEventArgs e)
        {
            CancelActivePolygon();

            activeMode = TOOL_MODE_TEXT;
            RefreshStatus();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            AlgoExecutionSteps.ItemsSource = null;
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();

            sfd.Title = "Choose file to save canvas contents";
            sfd.Filter = "PNG Image|*.png";

            if (sfd.ShowDialog() == true)
            {
                string fn = sfd.FileName;

                RenderVisualService.RenderToPNGFile(drawCanvas, fn);
            }
        }

        private void AlgoExecutionSteps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgoExecutionSteps.SelectedItem == null)
                return;

            var algoLayer = (AlgorithmStatusLayer)AlgoExecutionSteps.SelectedItem;

            CanvasWrapper.SwitchLayer(CanvasWrapper.LAYER_ALGORITHM);
            CanvasWrapper.ClearLayer();

            foreach (var c in algoLayer.Commands)
            {
                c.apply();
                c.applyToCanvas(CanvasWrapper);
            }
        }

        private void PolygonTool_Click(object sender, RoutedEventArgs e)
        {
            activeMode = TOOL_MODE_POLYGON;
            RefreshStatus();
        }

        private void LineTool_Click(object sender, RoutedEventArgs e)
        {
            CancelActivePolygon();

            activeMode = TOOL_MODE_LINE;
            RefreshStatus();
        }

        private void CreateAlgorithm()
        {
            associatedAlgorithm = AlgorithmFactory.CreateById(demo.AlgorithmId);
        }
        
        private void ComputeButton_Click(object sender, RoutedEventArgs e)
        {
            CancelActivePolygon();

            CreateAlgorithm();

            if (associatedAlgorithm == null)
                return;

            associatedAlgorithm.SetInputPoints(CanvasWrapper.InputPoints);
            associatedAlgorithm.SetInputPolygons(CanvasWrapper.InputPolygons);
            associatedAlgorithm.Run();

            if (associatedAlgorithm.History != null)
                AlgoExecutionSteps.ItemsSource = associatedAlgorithm.History.StatusLayers;
        }

        private void PointTool_Click(object sender, RoutedEventArgs e)
        {
            CancelActivePolygon();

            activeMode = TOOL_MODE_POINT;
            RefreshStatus();
        }

        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
            CancelActivePolygon();

            activeMode = TOOL_MODE_SELECTION;
            RefreshStatus();
        }
    }
}
