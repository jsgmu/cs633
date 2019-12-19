﻿using CompGeomVis.events;
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
    /// Interaction logic for InputPointSetsDisplayControl.xaml
    /// </summary>
    public partial class PointSetsDisplayControl : UserControl, IHandle<PointSetUpdated>
    {
        public PointSetsDisplayControl()
        {
            InitializeComponent();

            EventBus.Subscribe(this);
        }
        public void Handle(PointSetUpdated e)
        {
            TabItem existingTab = null;

            foreach(var t in PointSetsTabControl.Items)
            {
                var ti = (TabItem)t;

                if (ti.Header.Equals(e.Label))
                {
                    existingTab = ti;
                    break;
                }
            }

            PointGridDisplayControl grid = null;

            if (existingTab == null)
            {
                existingTab = new TabItem();
                existingTab.Header = e.Label;
                grid = new PointGridDisplayControl();
                existingTab.Content = grid;
                PointSetsTabControl.Items.Add(existingTab);
            } else
            {
                grid = (PointGridDisplayControl)existingTab.Content;
            }

            grid.SetPoints(e.Points);
        }
    }
}
