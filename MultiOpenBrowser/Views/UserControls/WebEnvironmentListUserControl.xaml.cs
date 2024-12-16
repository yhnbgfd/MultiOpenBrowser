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
            EventBus.OnWebEnvironmentListChange += EventBus_NotifyWebEnvironmentChange;
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

                // ALL分组
                {
                    WrapPanel wrapPanel = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                    };
                    var list = GlobalData.WebEnvironmentList.Select((value, i) => new { i, value });
                    foreach (var webEnv in list)
                    {
                        webEnv.value.Index = webEnv.i + 1;
                        wrapPanel.Children.Add(new WebEnvironmentListItemUserControl(webEnv.value));
                    }
                    TabItem tabItem = new()
                    {
                        IsSelected = true,
                        Content = new ScrollViewer() { Content = wrapPanel },
                    };
                    tabItem.SetDynamicResourceKey(TabItem.HeaderProperty, "Group_All");
                    this.TabControl_Group.Items.Add(tabItem);
                }

                // 自定义分组
                foreach (var group in GlobalData.WebEnvironmentGroupList)
                {
                    WrapPanel wrapPanel = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                    };
                    var list = GlobalData.WebEnvironmentList
                        .Where(a => a.WebEnvironmentGroupId == group.Id)
                        .Select((value, i) => new { i, value });
                    foreach (var webEnv in list)
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

                // 未分组
                {
                    WrapPanel wrapPanel = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                    };
                    var list = GlobalData.WebEnvironmentList
                        .Where(a => a.WebEnvironmentGroupId == null || !GlobalData.WebEnvironmentGroupList.Any(b => b.Id == a.WebEnvironmentGroupId))
                        .Select((value, i) => new { i, value });
                    foreach (var webEnv in list)
                    {
                        webEnv.value.Index = webEnv.i + 1;
                        wrapPanel.Children.Add(new WebEnvironmentListItemUserControl(webEnv.value));
                    }
                    TabItem tabItem = new()
                    {
                        Header = FindResource("Group_Other"),
                        Content = new ScrollViewer() { Content = wrapPanel },
                    };
                    tabItem.SetDynamicResourceKey(TabItem.HeaderProperty, "Group_Other");
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
