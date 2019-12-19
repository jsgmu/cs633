using CompGeomVis.events;
using CompGeomVis.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CompGeomVis.canvas
{
    public class CanvasWrapper
    {
        public List<CanvasLayer> Layers { get; set; }
        public Canvas AttachedCanvas { get; set; }
        public CanvasLayer ActiveLayer { get; set; }
        public CanvasConfiguration Config { get; set; }
        public List<Vector> InputPoints { get; set; }
        public List<LineModel> InputLines { get; set; }
        public List<PolygonModel> InputPolygons { get; set; }
        public PolygonModel ActivePolygon { get; set; }

        private CanvasComponent gridPointHighlight;
        private double highlightPointOffset = 0;

        public const int LAYER_BASE = 0;
        public const int LAYER_INPUT = 1;
        public const int LAYER_INPUT_ANNOTATION = 2;
        public const int LAYER_ALGORITHM = 3;
        public const int LAYER_ALGORITHM_ANNOTATION = 4;

        //public static 

        // Layer 0: Grid points / axis
        // Layer 1: Input Layer
        // Layer 2: Input Annotation Layer
        // Layer 3: Algorithm Layer
        // Layer 4: Algorithm Annotation Layer

        public CanvasWrapper(Canvas c, CanvasConfiguration config = null)
        {
            AttachedCanvas = c;

            Layers = new List<CanvasLayer>();

            CreateNewLayer("Base Grid Layer");
            ActiveLayer = Layers[0];
            CreateNewLayer("Input Layer");
            CreateNewLayer("Input Annotation Layer");
            CreateNewLayer("Algorithm Layer");
            CreateNewLayer("Algorithm Annotation Layer");

            //Config = new CanvasConfiguration();
            Config = config;
            //Config.BuildConfiguration(-200, 200, -100, 100, 
            //                          AttachedCanvas.ActualWidth, AttachedCanvas.ActualHeight,
            //                          40, 20, 5, 5);
            InputPoints = new List<Vector>();
            InputLines = new List<LineModel>();
            InputPolygons = new List<PolygonModel>();

            ActivePolygon = null;

            DrawGrid();
            HookEvents();
            CreateDefaultShapes();
        }

        public void SetAxis(AxisModel axis)
        {
            Config = new CanvasConfiguration();
            Config.BuildConfiguration(axis, AttachedCanvas.ActualWidth, AttachedCanvas.ActualHeight);
            DrawGrid();
            AdjustLocationsAfterResize();
        }

        public void LoadDemo(DemoModel demo)
        {
            if(demo.AxisConfig != null)
                SetAxis(demo.AxisConfig);

            // Clear canvas
            ClearLayers();
            // Clear all input structures
            ClearInput();
            // Re-execute demo input
            ActiveLayer = Layers[0];
        }

        private void ClearLayers()
        {
            for(int i = 1; i < Layers.Count; i++)
            {
                Layers[i].RemoveAllComponents();
            }
        }

        private void ClearInput()
        {
            InputPoints = new List<Vector>();
            InputLines = new List<LineModel>();
            InputPolygons = new List<PolygonModel>();
        }

        public void SetConfiguration(CanvasConfiguration c)
        {
            Config = c;
        }

        public void SwitchLayer(int layerId)
        {
            ActiveLayer = Layers[layerId];
        }

        public void ClearLayer()
        {
            ActiveLayer.RemoveAllComponents();
        }

        private CanvasLayer preservedLayer = null;
        public void PushActiveLayer()
        {
            preservedLayer = ActiveLayer;
        }
        public void PopActiveLayer()
        {
            if (preservedLayer != null)
                ActiveLayer = preservedLayer;
        }

        public void DrawPolygonUsingValues(PolygonModel poly)
        {
            foreach(var line in poly.Lines)
            {
                DrawLineUsingValues(line.StartPoint.X, line.StartPoint.Y, line.EndPoint.X, line.EndPoint.Y);
            }
        }

        public void DrawLineUsingValues(double xv1, double yv1, double xv2, double yv2)
        {
            // -------------------------------
            // works-ish
            double xrange = Math.Abs(Config.XMin) + Config.XMax;
            double yrange = Math.Abs(Config.YMin) + Config.YMax;

            double xper = (Config.CanvasWidth - Config.XStart) / xrange;
            double yper = (Config.CanvasHeight - Config.YStart) / yrange;

            var cp_x1 = (Math.Abs(Config.XMin) + xv1) * xper + Config.XStart;
            var cp_y1 = (Math.Abs(Config.YMax) - yv1) * yper + Config.YStart;

            var cp_x2 = (Math.Abs(Config.XMin) + xv2) * xper + Config.XStart;
            var cp_y2 = (Math.Abs(Config.YMax) - yv2) * yper + Config.YStart;
            // -------------------------------

            var line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Blue);
            line.StrokeThickness = 2;
            line.Fill = new SolidColorBrush(Colors.Blue);

            line.X1 = cp_x1;
            line.Y1 = cp_y1;
            line.X2 = cp_x2;
            line.Y2 = cp_y2;

            var cc = new CanvasComponent();
            cc.AddUiElement(line);

            ActiveLayer.AddComponent(cc);
        }

        public void HighlightPointUsingValues(double xv1, double yv1, int highlightLevel)
        {
            double xrange = Math.Abs(Config.XMin) + Config.XMax;
            double yrange = Math.Abs(Config.YMin) + Config.YMax;

            double xper = (Config.CanvasWidth - Config.XStart) / xrange;
            double yper = (Config.CanvasHeight - Config.YStart) / yrange;

            var cxp = (Math.Abs(Config.XMin) + xv1) * xper + Config.XStart;
            var cyp = (Math.Abs(Config.YMax) - yv1) * yper + Config.YStart;
            // -------------------------------

            double size = 12;
            double offset = size / 2;
            var ellipse = CreateEllipse(size, cxp - offset, cyp - offset);
            ellipse.Stroke = new SolidColorBrush(Colors.Yellow);

            var cc = new CanvasComponent();
            cc.AddUiElement(ellipse);
            cc.SendToFarBack();

            ActiveLayer.AddComponent(cc);
        }

        public void HighlightLineUsingValues(double xv1, double yv1, double xv2, double yv2)
        {
            // -------------------------------
            // works-ish
            double xrange = Math.Abs(Config.XMin) + Config.XMax;
            double yrange = Math.Abs(Config.YMin) + Config.YMax;

            double xper = (Config.CanvasWidth - Config.XStart) / xrange;
            double yper = (Config.CanvasHeight - Config.YStart) / yrange;

            var cp_x1 = (Math.Abs(Config.XMin) + xv1) * xper + Config.XStart;
            var cp_y1 = (Math.Abs(Config.YMax) - yv1) * yper + Config.YStart;

            var cp_x2 = (Math.Abs(Config.XMin) + xv2) * xper + Config.XStart;
            var cp_y2 = (Math.Abs(Config.YMax) - yv2) * yper + Config.YStart;
            // -------------------------------

            var line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Yellow);
            line.StrokeThickness = 6;
            line.Fill = new SolidColorBrush(Colors.Yellow);

            line.X1 = cp_x1;
            line.Y1 = cp_y1;
            line.X2 = cp_x2;
            line.Y2 = cp_y2;

            var cc = new CanvasComponent();
            cc.AddUiElement(line);
            cc.SendToFarBack();

            ActiveLayer.AddComponent(cc);
        }

        public void DrawLineUsingIndexes(int xi1, int yi1, int xi2, int yi2)
        {
            double xp1 = xi1 * Config.XIncrement + Config.XStart;
            double yp1 = yi1 * Config.YIncrement + Config.YStart;

            double xp2 = xi2 * Config.XIncrement + Config.XStart;
            double yp2 = yi2 * Config.YIncrement + Config.YStart;

            var line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 1;
            line.Fill = new SolidColorBrush(Colors.Red);

            line.X1 = xp1;
            line.Y1 = yp1;
            line.X2 = xp2;
            line.Y2 = yp2;
            line.Tag = new LineModel
            {
                StartPoint = new Vector(xp1, yp1, xi1, yi1),
                EndPoint = new Vector(xp2, yp2, xi2, yi2)
            };

            var cc = new CanvasComponent();
            cc.AddUiElement(line);

            ActiveLayer.AddComponent(cc);
        }
        private void CreateDefaultShapes()
        {
            gridPointHighlight = new CanvasComponent();

            double highlightPointSize = 9;

            highlightPointOffset = highlightPointSize / 2;

            Ellipse e = CreateEllipse(highlightPointSize, -10, -10);
            e.Stroke = new SolidColorBrush(Colors.Red);
            e.Visibility = Visibility.Collapsed;

            gridPointHighlight.AddUiElement(e);
            gridPointHighlight.SendToFarBack();

            GetBaseLayer().AddComponent(gridPointHighlight);
        }

        public CanvasLayer CreateNewLayer(string name)
        {
            var layer = new CanvasLayer(AttachedCanvas);

            layer.Name = name;
            Layers.Add(layer);

            return layer;
        }

        public void SetVisible(int layerIndex)
        {
            SetVisible(Layers[layerIndex]);
        }

        public void SetHidden(int layerIndex)
        {
            SetHidden(Layers[layerIndex]);
        }

        public void SetVisible(CanvasLayer layer)
        {
            layer.ShowLayer();
        }

        public void SetHidden(CanvasLayer layer)
        {
            layer.HideLayer();
        }

        public void AttachCanvas(Canvas canvas)
        {
            AttachedCanvas = canvas;

            DrawGrid();
            HookEvents();
        }

        private void HookEvents()
        {
            AttachedCanvas.SizeChanged += AttachedCanvas_SizeChanged;
            AttachedCanvas.MouseMove += AttachedCanvas_MouseMove;
            AttachedCanvas.MouseLeave += AttachedCanvas_MouseLeave;
            AttachedCanvas.MouseEnter += AttachedCanvas_MouseEnter;
            AttachedCanvas.MouseLeftButtonUp += AttachedCanvas_MouseLeftButtonUp;
        }

        private void UnhookEvents()
        {
            AttachedCanvas.SizeChanged -= AttachedCanvas_SizeChanged;
            AttachedCanvas.MouseMove -= AttachedCanvas_MouseMove;
            AttachedCanvas.MouseLeave -= AttachedCanvas_MouseLeave;
            AttachedCanvas.MouseLeftButtonUp -= AttachedCanvas_MouseLeftButtonUp;
        }

        private void AttachedCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void AttachedCanvas_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (gridPointHighlight != null)
            {
                gridPointHighlight.Show();
            }
        }

        private void AttachedCanvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(gridPointHighlight != null)
            {
                gridPointHighlight.Hide();
            }
        }

        private void AttachedCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var p = CoordHelper.TranslateFromMouse(Config, e.GetPosition(AttachedCanvas).X, e.GetPosition(AttachedCanvas).Y);

            double xp = p.DotIndexLeft * Config.XIncrement + Config.XStart;
            double yp = p.DotIndexTop * Config.YIncrement + Config.YStart;

            gridPointHighlight.Move(xp - highlightPointOffset, yp - highlightPointOffset);
        }

        private void AttachedCanvas_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if(Config != null) // this should only fail first time w/ no configuration
                Config.Recompute(AttachedCanvas.ActualWidth, AttachedCanvas.ActualHeight);

            DrawGrid();
            AdjustLocationsAfterResize();
        }

        // Shape drawing
        private Ellipse CreateEllipse(double size, double left, double top)
        {
            var dot = new Ellipse();

            dot.Stroke = new SolidColorBrush(Colors.Black);
            dot.StrokeThickness = 3;
            dot.Height = size;
            dot.Width = size;
            dot.Fill = new SolidColorBrush(Colors.Black);
            //dot.Margin = new Thickness(left, top, 0, 0);
            dot.SetValue(Canvas.LeftProperty, left);
            dot.SetValue(Canvas.TopProperty, top);

            return dot;
        }

        private System.Windows.Shapes.Line CreateLine(double x1, double y1, double x2, double y2)
        {
            var line = new System.Windows.Shapes.Line();

            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = 3;
            line.Fill = new SolidColorBrush(Colors.Black);
            line.X1 = x1;
            line.Y1 = y1;

            line.X2 = x2;
            line.Y2 = y2;

            return line;
        }

        // Core functionality (layer 0)
        CanvasComponent dotGrid = null;
        CanvasComponent axisLineX = null;
        CanvasComponent axisLineY = null;

        private CanvasLayer GetBaseLayer()
        {
            return Layers[0];
        }

        private void ResetGrid()
        {
            GetBaseLayer().RemoveComponent(dotGrid);

            dotGrid.Shapes.Clear();

            for(int yi = 0; yi < Config.YCount; yi++)
            {
                for(int xi = 0; xi < Config.XCount; xi++)
                {
                    double y = Config.YStart + yi * Config.YIncrement;
                    double x = Config.XStart + xi * Config.XIncrement;

                    dotGrid.Shapes.Add(CreateEllipse(3, x, y));
                }
            }

            GetBaseLayer().AddComponent(dotGrid);
        }

        private void AdjustLocationsAfterResize()
        {
            foreach(var layer in Layers)
            {
                foreach(var cc in layer.Components)
                {
                    foreach(var ui in cc.Shapes)
                    {
                        if(ui is Line)
                        {
                            var line = ui as Line;
                            var lineModel = line.Tag as LineModel;

                            if(lineModel != null)
                            {
                                line.X1 = Config.XStart + lineModel.StartPoint.Alternates.DotIndexLeft * Config.XIncrement;
                                line.Y1 = Config.YStart + lineModel.StartPoint.Alternates.DotIndexTop * Config.YIncrement;

                                line.X2 = Config.XStart + lineModel.EndPoint.Alternates.DotIndexLeft * Config.XIncrement;
                                line.Y2 = Config.YStart + lineModel.EndPoint.Alternates.DotIndexTop * Config.YIncrement;
                            }
                        } else if(ui is Ellipse)
                        {
                            var ellipse = ui as Ellipse;
                            var v = ellipse.Tag as Vector;

                            if(v != null)
                            {
                                var x = Config.XStart + v.Alternates.DotIndexLeft * Config.XIncrement + v.Alternates.DotLeftPostOffset;
                                var y = Config.YStart + v.Alternates.DotIndexTop * Config.YIncrement + v.Alternates.DotTopPostOffset;

                                ellipse.SetValue(Canvas.LeftProperty, x);
                                ellipse.SetValue(Canvas.TopProperty, y);
                            }
                        }
                    }
                }
            }
        }

        private void DrawGrid()
        {
            if(dotGrid == null)
            {
                dotGrid = new CanvasComponent();
            }

            if (Config == null)
            {
                // If we don't have a configuration yet, create the default...
                Config = new CanvasConfiguration();
                Config.BuildConfiguration(-800, 800, -400, 400,
                                          AttachedCanvas.ActualWidth, AttachedCanvas.ActualHeight,
                                          40, 20, 5, 5);
            }

            ResetGrid();

            double total_x = Math.Abs(Config.XMin) + Config.XMax;
            double total_y = Math.Abs(Config.YMin) + Config.YMax;
            double pct_x = Math.Abs(Config.XMin) / total_x;
            double pct_y = Math.Abs(Config.YMax) / total_y;
            int origin_xi = (int)(pct_x * (double)Config.XCount);
            int origin_yi = (int)(pct_y * (double)Config.YCount);
            double origin_xp = origin_xi * Config.XIncrement + Config.XStart + 1;
            double origin_yp = origin_yi * Config.YIncrement + Config.YStart + 1;

            var axisLine = CreateLine(0, origin_yp, AttachedCanvas.ActualWidth, origin_yp);
            axisLine.Stroke = new SolidColorBrush(Colors.Black);

            if (axisLineX != null)
            {
                GetBaseLayer().RemoveComponent(axisLineX);
                axisLineX.Shapes.Clear();
            } else
            {
                axisLineX = new CanvasComponent();
            }

            axisLineX.AddUiElement(axisLine);
            GetBaseLayer().AddComponent(axisLineX);

            axisLine = CreateLine(origin_xp, 0, origin_xp, AttachedCanvas.ActualHeight);
            axisLine.Stroke = new SolidColorBrush(Colors.Black);

            if(axisLineY != null)
            {
                GetBaseLayer().RemoveComponent(axisLineY);
                axisLineY.Shapes.Clear();
            }
            else
            {
                axisLineY = new CanvasComponent();
            }

            axisLineY.AddUiElement(axisLine);
            GetBaseLayer().AddComponent(axisLineY);
        }
        // end core

        // Input Layer Related
        private CanvasLayer GetInputLayer()
        {
            return Layers[1];
        }

        private CanvasLayer GetInputAnnotationLayer()
        {
            return Layers[2];
        }

        public void AddInputPoint(int xi, int yi)
        {
            double xp = xi * Config.ValueTickX + Config.XStart;
            double yp = yi * Config.ValueTickY + Config.YStart;

            AddInputPoint(xi, yi, xp, yp);
        }

        public void AddInputPoint(int xi, int yi, double xp, double yp)
        {
            double cxp = xi * Config.XIncrement + Config.XStart;
            double cyp = yi * Config.YIncrement + Config.YStart;

            InputPoints.Add(new Vector { X = xp, Y = yp, HighlightLevel = 1,
                Alternates = new CanvasPoint { DotIndexLeft = xi, DotIndexTop = yi, DotTopPostOffset = -3, DotLeftPostOffset = -3 } });

            if(InputPoints.Count > 0)
            {
                InputPoints[InputPoints.Count - 1].HighlightLevel = 0;
            }

            EventBus.Publish<PointSetUpdated>(new PointSetUpdated
            {
                Label = "Raw Input",
                Points = InputPoints
            });

            double size = 6;
            double offset = size / 2;
            var ellipse = CreateEllipse(size, cxp - offset, cyp - offset);
            ellipse.Tag = InputPoints[InputPoints.Count - 1];
            ellipse.Stroke = new SolidColorBrush(Colors.Blue);

            var cc = new CanvasComponent();
            cc.AddUiElement(ellipse);

            GetInputLayer().AddComponent(cc);
        }

        public void AddInputLine(int xi1, int yi1, int xi2, int yi2)
        {
            double xp1 = xi1 * Config.XIncrement + Config.XStart;
            double yp1 = yi1 * Config.YIncrement + Config.YStart;
            double xp2 = xi2 * Config.XIncrement + Config.XStart;
            double yp2 = yi2 * Config.YIncrement + Config.YStart;

            if (InputLines.Count > 0)
            {
                InputLines[InputLines.Count - 1].HighlightLevel = 0;
            }

            EventBus.Publish<LineSetUpdated>(new LineSetUpdated
            {
                Label = "Raw Input",
                Lines = InputLines
            });

            InputLines.Add(new LineModel { StartPoint = new Vector(xp1, yp1, xi1, yi1),
                                           EndPoint = new Vector(xp2, yp2, xi2, yi2),
                                           HighlightLevel = 1});

            var line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 3;
            line.Fill = new SolidColorBrush(Colors.Red);
            line.Tag = InputLines[InputLines.Count - 1];
            line.X1 = xp1;
            line.Y1 = yp1;
            line.X2 = xp2;
            line.Y2 = yp2;

            var cc = new CanvasComponent();
            cc.AddUiElement(line);

            GetInputLayer().AddComponent(cc);
        }

        private List<CanvasComponent> polygonLinesInProgress = new List<CanvasComponent>();
        public void CancelActivePolygon()
        {
            if (ActivePolygon == null)
                return;

            foreach (var cc in polygonLinesInProgress)
            {
                GetInputLayer().RemoveComponent(cc);
            }

            polygonLinesInProgress.Clear();
            ActivePolygon = null;
        }

        private void CompletePolygon()
        {
            if (ActivePolygon == null || !ActivePolygon.IsClosed)
                return;

            InputPolygons.Add(ActivePolygon);

            polygonLinesInProgress.Clear();
            ActivePolygon = null;
        }

        public PolygonModel LastPolygon()
        {
            if (InputPolygons.Count == 0)
                return null;

            return InputPolygons[InputPolygons.Count - 1];
        }

        public bool PolygonJustCompleted()
        {
            return ActivePolygon == null;
        }

        public void AddPolygonLine(int xi1, int yi1, int xi2, int yi2,
                                   double xv1, double yv1, double xv2, double yv2)
        {
            if (ActivePolygon == null)
            {
                ActivePolygon = new PolygonModel();
            }

            double xp1 = xi1 * Config.XIncrement + Config.XStart;
            double yp1 = yi1 * Config.YIncrement + Config.YStart;
            double xp2 = xi2 * Config.XIncrement + Config.XStart;
            double yp2 = yi2 * Config.YIncrement + Config.YStart;

            var line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 3;
            line.Fill = new SolidColorBrush(Colors.Red);
            line.X1 = xp1;
            line.Y1 = yp1;
            line.X2 = xp2;
            line.Y2 = yp2;

            ActivePolygon.Lines.Add(new LineModel
            {
                StartPoint = new Vector { X = xv1, Y = yv1, Alternates = new CanvasPoint { DotIndexLeft = xi1, DotIndexTop = yi1 } },
                EndPoint = new Vector { X = xv2, Y = yv2, Alternates = new CanvasPoint { DotIndexLeft = xi2, DotIndexTop = yi2 } }
            } );

            var cc = new CanvasComponent();
            cc.AddUiElement(line);

            line.Tag = ActivePolygon.Lines[ActivePolygon.Lines.Count - 1];

            polygonLinesInProgress.Add(cc);

            GetInputLayer().AddComponent(cc);

            // if we're at least completing a triangle...
            if (ActivePolygon.Lines.Count > 2)
            {
                // if endpoint of current line matches start point of first line, we're connecting
                // back to start of polygon
                if (GeomMath.AlmostEqual(new Vector { X = xv2, Y = yv2 }, ActivePolygon.Lines[0].StartPoint))
                {
                    CompletePolygon();
                }
            }
        }
        // -------------------------
    }
}
