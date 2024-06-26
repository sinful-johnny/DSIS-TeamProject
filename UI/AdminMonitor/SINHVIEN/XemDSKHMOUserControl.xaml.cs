﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AdminMonitor.SINHVIEN
{
    /// <summary>
    /// Interaction logic for XemDSKHMOUserControl.xaml
    /// </summary>
    public partial class XemDSKHMOUserControl : UserControl
    {
        OracleConnection Conn;
        public XemDSKHMOUserControl(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadingProgressBar.IsIndeterminate = false;
            loadingProgressBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingProgressBar.Value = 40;

            List<KHMO>? data = null;
            await Task.Run(() => {
                data = Controller_KHMO.GetKHMO(Conn);
            });
            MainDataGrid.ItemsSource = data;

            await Task.Run(() => Thread.Sleep(25));
            loadingProgressBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingProgressBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));

            loadingProgressBar.IsIndeterminate = true;
        }
    }
}
