using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        internal static List<WebEnvironment> WebEnvironmentList => GlobalData.WebEnvironmentList;

        public WebEnvironmentListUserControl()
        {
            InitializeComponent();
            DataContext = this;
            EventBus.NotifyWebEnvironmentChange += EventBus_NotifyWebEnvironmentChange;
        }

        private async Task EventBus_NotifyWebEnvironmentChange()
        {
            await ReloadListAsync();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await EventBus_NotifyWebEnvironmentChange();
        }

        private async Task ReloadListAsync()
        {
            try
            {
                WebEnvironmentRepo webEnvironmentRepo = new(null);
                GlobalData.WebEnvironmentList = await webEnvironmentRepo.Select
                    .LeftJoin(a => a.WebBrowser != null && a.WebBrowserId == a.WebBrowser.Id)
                    .OrderBy(a => a.Order)
                    .OrderBy(a => a.Id)
                    .ToListAsync();

                _logger.Info($"WebEnvironmentList Count={WebEnvironmentList.Count}");

                this.StackPanel_WebEnvironmentList.Children.Clear();
                foreach (var item in WebEnvironmentList.Select((value, i) => new { i, value }))
                {
                    item.value.Index = item.i + 1;
                    WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(item.value);
                    this.StackPanel_WebEnvironmentList.Children.Add(webEnvironmentListItemUserControl);
                }

                JumpListHelper.SetJumpList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
