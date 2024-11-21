using System.Windows;
using System.Windows.Controls;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebEnvironmentListUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironmentListUserControl()
        {
            InitializeComponent();
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
                await WebEnvironmentGroupRepo.LoadAsync();
                await WebEnvironmentRepo.LoadAsync();

                _logger.Info($"WebEnvironmenGrouptList Count={GlobalData.WebEnvironmentGroupList.Count}");
                _logger.Info($"WebEnvironmentList Count={GlobalData.WebEnvironmentList.Count}");

                this.StackPanel_WebEnvironmentList.Children.Clear();
                foreach (var webEnv in GlobalData.WebEnvironmentList.Select((value, i) => new { i, value }))
                {
                    webEnv.value.Index = webEnv.i + 1;
                    WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(webEnv.value);
                    this.StackPanel_WebEnvironmentList.Children.Add(webEnvironmentListItemUserControl);
                }

                foreach (var group in GlobalData.WebEnvironmentGroupList)
                {
                    WrapPanel wrapPanel = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                    };
                    TabItem tabItem = new()
                    {
                        Header = group.Name,
                        Content = new ScrollViewer()
                        {
                            Content = wrapPanel,
                        },
                    };

                    foreach (var webEnv in GlobalData.WebEnvironmentList.Where(a => a.WebEnvironmentGroupId == group.Id).Select((value, i) => new { i, value }))
                    {
                        webEnv.value.Index = webEnv.i + 1;
                        WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(webEnv.value);
                        wrapPanel.Children.Add(webEnvironmentListItemUserControl);
                    }

                    this.TabControl_Group.Items.Add(tabItem);
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
