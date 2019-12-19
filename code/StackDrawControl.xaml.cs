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
    /// Interaction logic for StackDrawControl.xaml
    /// </summary>
    public partial class StackDrawControl : UserControl
    {
        private Stack<Vector> vectorStack;

        public StackDrawControl()
        {
            InitializeComponent();

            vectorStack = new Stack<Vector>();
            vectorStack.Push(new Vector { X = 0, Y = 0 });
            vectorStack.Push(new Vector { X = 10, Y = 10 });
            vectorStack.Push(new Vector { X = 20, Y = 20 });
            vectorStack.Push(new Vector { X = 30, Y = 30 });
            vectorStack.Push(new Vector { X = 40, Y = 40 });

            DrawStack();
        }

        private void DrawStack()
        {
            double sx = 50;
            double sy = 10;
            double x = sx;
            double y = sy;

            //for(int i = 0; i < vectorStack.Count; i++)
            int i = 0;
            foreach(var v in vectorStack)
            {
                y = sy + 15 * i;
                var tb = new TextBlock();
                tb.Text = "(" + v.X + ", " + v.Y + ")";
                tb.SetValue(Canvas.LeftProperty, x);
                tb.SetValue(Canvas.TopProperty, y);

                //if (i == 0)
                //{
                //    tb.Background = new SolidColorBrush(Colors.Yellow);
                //}

                rootCanvas.Children.Add(tb);
                i++;
            }

            double ey = sy + 15 * i + 5;
            double ex = sx + 55;

            sx -= 5;

            var line = new Line();
            line.X1 = sx;
            line.Y1 = sy;
            line.X2 = sx;
            line.Y2 = ey;
            line.Stroke = new SolidColorBrush(Colors.Black);
            rootCanvas.Children.Add(line);

            line = new Line();
            line.X1 = sx;
            line.Y1 = ey;
            line.X2 = ex;
            line.Y2 = ey;
            line.Stroke = new SolidColorBrush(Colors.Black);
            rootCanvas.Children.Add(line);

            line = new Line();
            line.X1 = ex;
            line.Y1 = sy;
            line.X2 = ex;
            line.Y2 = ey;
            line.Stroke = new SolidColorBrush(Colors.Black);
            rootCanvas.Children.Add(line);
        }
    }
}
