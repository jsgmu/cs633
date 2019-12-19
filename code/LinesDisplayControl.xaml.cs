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
    /// Interaction logic for LinesDisplayControl.xaml
    /// </summary>
    public partial class LinesDisplayControl : UserControl, IHandle<LineSetUpdated>
    {
        public LinesDisplayControl()
        {
            InitializeComponent();

            EventBus.Subscribe(this);
        }

        public void Handle(LineSetUpdated e)
        {
            TabItem existingTab = null;

            foreach (var t in LineSetsTabControl.Items)
            {
                var ti = (TabItem)t;

                if (ti.Header.Equals(e.Label))
                {
                    existingTab = ti;
                    break;
                }
            }

            if (existingTab == null)
            {
                existingTab = new TabItem();
                existingTab.Header = e.Label;
                var grid = new LinesGridControl();
                grid.SetLines(e.Lines);
                existingTab.Content = grid;
                LineSetsTabControl.Items.Add(existingTab);
            }
            else
            {
                var grid = (LinesGridControl)existingTab.Content;
                grid.SetLines(e.Lines);
            }
        }
    }
}
