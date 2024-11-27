using System.Windows;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class OptionsWindow : Window
    {
        public static Option? Option => GlobalData.Option;

        public OptionsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            await CacheRepo.SetAsync("Option", Option, null);
            this.Close();
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
