using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class WindowHelper
    {
        public static MainWindow MainWindow { get; set; }

        public static void CreateNewDemoWindow()
        {
            var w = new DemoConfigWindow();

            w.Owner = MainWindow;
            w.ShowDialog();
        }
    }
}
