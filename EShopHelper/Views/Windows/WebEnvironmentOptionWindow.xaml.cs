using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class WebEnvironmentOptionWindow : Window
    {
        public WebEnvironmentOptionWindow()
        {
            InitializeComponent();
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            WebEnvironment webEnvironment = new()
            {
                Name = "环境1",
            };

            WebEnvironmentRepo webEnvironmentRepo = new(null);
            await webEnvironmentRepo.InsertAsync(webEnvironment);

            this.Close();
        }
    }
}
