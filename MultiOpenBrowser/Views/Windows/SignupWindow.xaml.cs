﻿using System.Windows;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class SignupWindow : Window
    {
        public SignupWindow()
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