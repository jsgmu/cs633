using CompGeomVis.datasources;
using CompGeomVis.events;
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
    /// Interaction logic for PointDisplayControl.xaml
    /// </summary>
    public partial class PointGridDisplayControl : UserControl
    {
        private PointsDataSource PointsDataProvider { get; set; }

        public PointGridDisplayControl()
        {
            InitializeComponent();

            PointsDataProvider = new PointsDataSource();
            ObjectDataProvider o = new ObjectDataProvider();
            o.ObjectInstance = PointsDataProvider;
            o.MethodName = "GetPoints";
            mainGrid.DataContext = o;
        }

        public void SetPoints(List<Vector> points)
        {
            mainGrid.DataContext = null;
            PointsDataProvider.UpdatePoints(points);
            ObjectDataProvider o = new ObjectDataProvider();
            o.ObjectInstance = PointsDataProvider;
            o.MethodName = "GetPoints";
            mainGrid.DataContext = o;
        }
    }
}
