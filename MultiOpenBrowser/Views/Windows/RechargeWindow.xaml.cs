﻿using System.Windows;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class RechargeWindow : Window
    {
        public RechargeWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}