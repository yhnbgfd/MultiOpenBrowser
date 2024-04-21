using NLog;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; }

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            DataContext = this;
            WebEnvironment = webEnvironment;
        }

        private void Button_StartWebEnvironment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                WebEnvironment.StartWebBrowser();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
