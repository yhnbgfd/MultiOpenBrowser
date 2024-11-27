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

                this.TabControl_Group.Items.Clear();
                {
                    WrapPanel wrapPanel = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                    };
                    foreach (var webEnv in GlobalData.WebEnvironmentList.Select((value, i) => new { i, value }))
                    {
                        webEnv.value.Index = webEnv.i + 1;
                        wrapPanel.Children.Add(new WebEnvironmentListItemUserControl(webEnv.value));
                    }
                    this.TabControl_Group.Items.Add(new TabItem()
                    {
                        IsSelected = true,
                        Header = "ALL",
                        Content = new ScrollViewer() { Content = wrapPanel },
                    });
                }

                foreach (var group in GlobalData.WebEnvironmentGroupList)
                {
                    WrapPanel wrapPanel = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                    };
                    foreach (var webEnv in GlobalData.WebEnvironmentList.Where(a => a.WebEnvironmentGroupId == group.Id).Select((value, i) => new { i, value }))
                    {
                        webEnv.value.Index = webEnv.i + 1;
                        wrapPanel.Children.Add(new WebEnvironmentListItemUserControl(webEnv.value));
                    }
                    this.TabControl_Group.Items.Add(new TabItem()
                    {
                        Header = group.Name,
                        Content = new ScrollViewer() { Content = wrapPanel },
                    });
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
