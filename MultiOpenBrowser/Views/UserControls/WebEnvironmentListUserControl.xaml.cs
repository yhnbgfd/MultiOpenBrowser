using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebEnvironmentListUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private WrapPanel _wrapPanel_All = new();

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
                    _wrapPanel_All = new()
                    {
                        Margin = new Thickness(5, 0, 5, 0),
                        AllowDrop = true,
                    };
                    var list = GlobalData.WebEnvironmentList.Select((value, i) => new { i, value });
                    foreach (var webEnv in list)
                    {
                        webEnv.value.Index = webEnv.i + 1;
                        WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(webEnv.value);
                        webEnvironmentListItemUserControl.MouseLeftButtonDown += WebEnvironmentListItemUserControl_MouseLeftButtonDown;
                        webEnvironmentListItemUserControl.DragOver += WebEnvironmentListItemUserControl_DragOver;
                        webEnvironmentListItemUserControl.Drop += WebEnvironmentListItemUserControl_Drop;
                        _wrapPanel_All.Children.Add(webEnvironmentListItemUserControl);
                    }
                    TabItem tabItem = new()
                    {
                        IsSelected = true,
                        Content = new ScrollViewer() { Content = _wrapPanel_All },
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

        private async void WebEnvironmentListItemUserControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(WebEnvironmentListItemUserControl)) is WebEnvironmentListItemUserControl draggedItem)
            {
                // 获取被拖动的项的索引
                int draggedIndex = _wrapPanel_All.Children.IndexOf(draggedItem);
                if (sender is WebEnvironmentListItemUserControl targetItem)
                {
                    // 获取目标项的索引
                    int targetIndex = _wrapPanel_All.Children.IndexOf(targetItem);

                    // 更新 WrapPanel 中的子项顺序
                    if (draggedIndex != targetIndex)
                    {
                        _wrapPanel_All.Children.Remove(draggedItem);
                        _wrapPanel_All.Children.Insert(targetIndex, draggedItem);

                        // 保存到数据库
                        foreach (var item in _wrapPanel_All.Children)
                        {
                            if (item is WebEnvironmentListItemUserControl webEnvUC)
                            {
                                var newOrder = _wrapPanel_All.Children.IndexOf(webEnvUC);
                                if (webEnvUC.WebEnvironment.Order != newOrder)
                                {
                                    webEnvUC.WebEnvironment.Order = _wrapPanel_All.Children.IndexOf(webEnvUC);
                                    _ = new WebEnvironmentRepo(null).InsertOrUpdateAsync(webEnvUC.WebEnvironment);
                                }
                            }
                        }

                        await ReloadListAsync();
                    }
                }
            }
        }

        private void WebEnvironmentListItemUserControl_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void WebEnvironmentListItemUserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is WebEnvironmentListItemUserControl draggedItem)
            {
                DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Move);
            }
        }
    }
}
