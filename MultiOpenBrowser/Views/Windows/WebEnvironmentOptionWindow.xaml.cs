using MultiOpenBrowser.Base;
using MultiOpenBrowser.Entitys;
using MultiOpenBrowser.Helpers;
using MultiOpenBrowser.Repositorys;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class WebEnvironmentOptionWindow : Window, INotifyPropertyChanged
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; } = WebEnvironment.Default;
        public WebBrowser? WebBrowser { get; set; }

        public WebEnvironmentOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WebBrowser ??= this.WebEnvironment.WebBrowser;
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            using var uow = Global.FSql.CreateUnitOfWork();
            try
            {
                WebBrowserRepo webBrowserRepo = new(uow);
                WebEnvironment.WebBrowser = await webBrowserRepo.InsertOrUpdateAsync(WebBrowser!);
                WebEnvironment.WebBrowserId = WebEnvironment.WebBrowser!.Id;

                WebEnvironmentRepo webEnvironmentRepo = new(uow);
                await webEnvironmentRepo.InsertOrUpdateAsync(WebEnvironment);

                uow.Commit();

                JumpListHelper.SetJumpList();

                EventBus.NotifyWebEnvironmentChange?.Invoke();

                this.Close();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
            }
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
