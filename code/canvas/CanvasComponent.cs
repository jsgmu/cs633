using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CompGeomVis.canvas
{
    public class CanvasComponent
    {
        public List<UIElement> Shapes { get; set; }
        public Dictionary<UIElement, String> Annotations { get; set; }

        // Z more negative = further behind
        public const int Z_FARPLUSONE = -101;
        public const int Z_FAR = -100;
        public const int Z_NEUTRAL = 0;
        public const int Z_NEAR = 100;
        public const int Z_NEARPLUSONE = 101;

        public CanvasComponent()
        {
            Shapes = new List<UIElement>();
            Annotations = new Dictionary<UIElement, string>();
        }

        public void Hide()
        {
            foreach(var s in Shapes)
            {
                s.SetValue(FrameworkElement.VisibilityProperty, Visibility.Collapsed);
            }
        }

        public void Show()
        {
            foreach (var s in Shapes)
            {
                s.SetValue(FrameworkElement.VisibilityProperty, Visibility.Visible);
            }
        }

        public void AddUiElement(UIElement u, string annotation)
        {
            Shapes.Add(u);
            Annotations.Add(u, annotation);
        }

        public void AddUiElement(UIElement u)
        {
            Shapes.Add(u);
        }

        public void SendToFarBack()
        {
            foreach(var s in Shapes)
            {
                s.SetValue(Canvas.ZIndexProperty, Z_FARPLUSONE);
            }
        }

        public void Move(double x, double y)
        {
            foreach(var s in Shapes)
            {
                s.SetValue(Canvas.LeftProperty, x);
                s.SetValue(Canvas.TopProperty, y);
            }
        }
    }
}
