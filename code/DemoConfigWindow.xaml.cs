using CompGeomVis.events;
using CompGeomVis.models;
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
using System.Windows.Shapes;

namespace CompGeomVis
{
    /// <summary>
    /// Interaction logic for DemoConfigWindow.xaml
    /// </summary>
    public partial class DemoConfigWindow : Window
    {
        public DemoConfigWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var demo = new DemoModel();

            var cbi = AlgoCombo.SelectedItem as ComboBoxItem;
            demo.AlgorithmId = Int32.Parse(cbi.Name.Substring(4));
            demo.AlgorithmIndex = AlgoCombo.SelectedIndex;
            demo.Title = FullName.Text;
            demo.ShortTitle = ShortName.Text;

            double xmin = Double.Parse(xMinText.Text);
            double xmax = Double.Parse(xMaxText.Text);

            double ymin = Double.Parse(yMinText.Text);
            double ymax = Double.Parse(yMaxText.Text);

            double xstart = Double.Parse(xStartText.Text);
            double ystart = Double.Parse(yStartText.Text);

            int xCount = Int32.Parse(xCountText.Text);
            int yCount = Int32.Parse(yCountText.Text);

            demo.AxisConfig = new AxisModel { XMin = xmin, XMax = xmax, YMin = ymin, YMax = ymax,
                                              XStart = xstart, YStart = ystart, XCount = xCount, YCount = yCount };

            EventBus.Publish<DemoCreated>(new DemoCreated { Demo = demo });

            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
