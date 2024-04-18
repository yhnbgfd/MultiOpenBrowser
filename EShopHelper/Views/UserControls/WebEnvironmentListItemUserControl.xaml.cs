using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        public WebEnvironment WebEnvironment { get; set; } = new();

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            DataContext = this;
            WebEnvironment = webEnvironment;
        }

        private void Button_StartWebEnvironment_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
