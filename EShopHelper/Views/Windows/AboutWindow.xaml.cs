using System.Windows;
using System.Windows.Input;

namespace EShopHelper.Views.Windows
{
    public partial class AboutWindow : Window
    {
        public string? AppVersion => GlobalData.AppVersion;

        public AboutWindow()
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
