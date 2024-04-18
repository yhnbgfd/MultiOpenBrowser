using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListUserControl : UserControl
    {
        internal List<WebEnvironment> Data { get; set; } = [];

        public WebEnvironmentListUserControl()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WebEnvironmentRepo webEnvironmentRepo = new(null);
            Data = await webEnvironmentRepo.Select.ToListAsync();
        }
    }
}
