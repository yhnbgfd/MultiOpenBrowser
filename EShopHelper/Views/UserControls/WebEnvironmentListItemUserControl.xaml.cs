using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        internal WebEnvironment WebEnvironment { get; set; } = new();

        public WebEnvironmentListItemUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
