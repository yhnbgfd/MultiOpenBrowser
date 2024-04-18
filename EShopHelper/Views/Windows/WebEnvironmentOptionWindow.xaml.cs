using System.Windows;

namespace EShopHelper.Views.Windows
{
    public partial class WebEnvironmentOptionWindow : Window
    {
        internal WebEnvironment WebEnvironment { get; set; } = new();

        public WebEnvironmentOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            WebEnvironment = new()
            {
                Name = "环境1",
            };

            WebEnvironmentRepo webEnvironmentRepo = new(null);
            await webEnvironmentRepo.InsertOrUpdateAsync(WebEnvironment);

            this.Close();
        }
    }
}
