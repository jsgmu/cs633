using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CompGeomVis.canvas
{
    public class CanvasLayer
    {
        public string Name { get; set; }
        public List<CanvasComponent> Components { get; private set; }
        public bool IsVisible { get; private set; }
        public bool IsStatic { get; set; }
        public bool IsAnnotation { get; set; }
        public Canvas AttachedCanvas { get; private set; }
        public string Commentary { get; set; }

        public CanvasLayer(Canvas canvas)
        {
            AttachedCanvas = canvas;
            IsVisible = true;
            IsStatic = false;
            IsAnnotation = false;
            Name = "Unnamed";
            Components = new List<CanvasComponent>();
        }

        public void AttachCanvas(Canvas canvas)
        {
            AttachedCanvas = canvas;
        }

        public void ShowLayer()
        {
            if (IsVisible)
                return;

            foreach (var c in Components)
            {
                foreach (var s in c.Shapes)
                {
                    AttachedCanvas.Children.Add(s);
                }
            }

            IsVisible = true;
        }

        public void HideLayer()
        {
            if (!IsVisible)
                return;

            foreach (var c in Components)
            {
                foreach (var s in c.Shapes)
                {
                    AttachedCanvas.Children.Remove(s);
                }
            }

            IsVisible = false;
        }

        public void AddComponent(CanvasComponent c)
        {
            Components.Add(c);

            if(IsVisible)
            {
                foreach(var s in c.Shapes)
                {
                    AttachedCanvas.Children.Add(s);
                }
            }
        }

        public void RemoveComponent(CanvasComponent c)
        {
            if (!Components.Contains(c))
                return;

            foreach (var s in c.Shapes)
            {
                AttachedCanvas.Children.Remove(s);
            }

            Components.Remove(c);
        }

        public void RemoveAllComponents()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                var c = Components[i];

                RemoveComponent(c);
            }
        }
    }
}
