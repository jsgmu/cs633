using CompGeomVis.datasources;
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
    /// Interaction logic for LinesGridControl.xaml
    /// </summary>
    public partial class LinesGridControl : UserControl
    {
        private LinesDataSource DataSource { get; set; }

        public LinesGridControl()
        {
            InitializeComponent();

            DataSource = new LinesDataSource();
            ObjectDataProvider o = new ObjectDataProvider();
            o.ObjectInstance = DataSource;
            o.MethodName = "GetLines";
            mainGrid.DataContext = o;
        }

        public void SetLines(List<LineModel> lines)
        {
            DataSource.UpdateLines(lines);
        }
    }
}
