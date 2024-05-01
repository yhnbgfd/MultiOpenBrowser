using NLog;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        internal List<WebEnvironment> WebEnvironmentList { get; set; } = [];

        public WebEnvironmentListUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await ReloadListAsync();
        }

        public async Task ReloadListAsync()
        {
            try
            {
                WebEnvironmentRepo webEnvironmentRepo = new(null);
                WebEnvironmentList = await webEnvironmentRepo.Select
                    .LeftJoin(a => a.WebBrowser != null && a.WebBrowserId == a.WebBrowser.Id)
                    .OrderBy(a => a.Order)
                    .OrderBy(a => a.Id)
                    .ToListAsync();

                _logger.Info($"WebEnvironmentList Count={WebEnvironmentList.Count}");

                JumpList jumpList = new()
                {
                    ShowFrequentCategory = false,
                    ShowRecentCategory = false
                };

                this.StackPanel_WebEnvironmentList.Children.Clear();
                foreach (var item in WebEnvironmentList.Select((value, i) => new { i, value }))
                {
                    item.value.Index = item.i + 1;
                    WebEnvironmentListItemUserControl webEnvironmentListItemUserControl = new(item.value)
                    {
                        Margin = new Thickness(5),
                    };
                    webEnvironmentListItemUserControl.DeleteClick += WebEnvironmentListItemUserControl_DeleteClick;
                    this.StackPanel_WebEnvironmentList.Children.Add(webEnvironmentListItemUserControl);

                    JumpTask task = new()
                    {
                        Title = item.value.Name,
                        Arguments = item.value.Id.ToString(),
                        Description = item.value.Name,
                        CustomCategory = "WebEnvironment List",
                        IconResourcePath = Environment.ProcessPath,
                        ApplicationPath = Environment.ProcessPath,
                        WorkingDirectory = Directory.GetCurrentDirectory(),
                    };
                    jumpList.JumpItems.Add(task);
                }

                JumpList.SetJumpList(Application.Current, jumpList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async void WebEnvironmentListItemUserControl_DeleteClick(object sender, RoutedEventArgs e)
        {
            await ReloadListAsync();
        }
    }
}
