﻿using Oracle.ManagedDataAccess.Client;
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

namespace AdminMonitor.GIAOVU
{
    /// <summary>
    /// Interaction logic for ThemSuaHocPhanWindow.xaml
    /// </summary>
    public partial class ThemSuaHocPhanWindow : Window
    {
        class Modes
        {
            public static int Insert => 0;
            public static int Update => 1;
        }

        OracleConnection Conn;
        HocPhan? _TTHocPhan;
        int mode = Modes.Insert;
        public ThemSuaHocPhanWindow(OracleConnection connection, HocPhan data)
        {
            InitializeComponent();
            Conn = connection;
            _TTHocPhan = data;
            mode = Modes.Update;
            DataContext = _TTHocPhan;
            mainTitle.Content = "Chỉnh sửa thông tin học phần";
            this.Title = "Chỉnh sửa thông tin học phần";
            MaHocPhanTextBox.IsEnabled = false;
        }
        public ThemSuaHocPhanWindow(OracleConnection connection)
        {
            InitializeComponent();
            Conn = connection;
            mode = Modes.Insert;
            _TTHocPhan = new HocPhan();
            DataContext = _TTHocPhan;
            mainTitle.Content = "Thêm học phần";
            this.Title = "Thêm học phần";
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;

            SaveButton.IsEnabled = false;
            loadingBar.IsIndeterminate = false;
            loadingBar.Value = 10;
            await Task.Run(() => Thread.Sleep(10));
            loadingBar.Value = 40;

            try
            {
                if (_TTHocPhan != null)
                {
                    if (mode == Modes.Update)
                    {
                        await Task.Run(() => Controller_HocPhan.UpdateHocPhan(Conn, _TTHocPhan));
                    }
                    else if (mode == Modes.Insert)
                    {
                        await Task.Run(() => Controller_HocPhan.InsertHocPhan(Conn, _TTHocPhan));
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
                MessageBox.Show(ex.ToString(), "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await Task.Run(() => Thread.Sleep(25));
            loadingBar.Value = 80;
            await Task.Run(() => Thread.Sleep(50));
            loadingBar.Value = 100;
            await Task.Run(() => Thread.Sleep(25));
            if (flag)
            {
                MessageBox.Show("Sửa thông tin học phần thành công!", "Thành công", MessageBoxButton.OK);
                loadingBar.IsIndeterminate = true;
                SaveButton.IsEnabled = true;
                DialogResult = true;
            }
        }

        private void CancelButotn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
