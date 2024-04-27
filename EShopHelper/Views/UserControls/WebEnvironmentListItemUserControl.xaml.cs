using EShopHelper.Views.Windows;
using NLog;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment? WebEnvironment { get; set; }

        public static readonly RoutedEvent DeleteClickEvent = EventManager.RegisterRoutedEvent(
            name: nameof(DeleteClick),
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WebEnvironmentListItemUserControl));

        public event RoutedEventHandler DeleteClick
        {
            add { AddHandler(DeleteClickEvent, value); }
            remove { RemoveHandler(DeleteClickEvent, value); }
        }

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            DataContext = this;
            WebEnvironment = webEnvironment;
        }

        void RaiseDeleteClickEvent()
        {
            RoutedEventArgs routedEventArgs = new(routedEvent: DeleteClickEvent);
            RaiseEvent(routedEventArgs);
        }

        private void Button_StartWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebEnvironment?.StartWebBrowser();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async void Button_DeleteWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            if (WebEnvironment == null)
            {
                return;
            }

            var result = MessageBox.Show(Application.Current.MainWindow, "Delete WebEnvironment ?", "Delete WebEnvironment", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

            using var uow = Global.FSql.CreateUnitOfWork();
            try
            {
                if (WebEnvironment.WebBrowser?.IsTemplate == false)
                {
                    WebBrowserRepo webBrowserRepo = new(uow);
                    await webBrowserRepo.DeleteAsync(WebEnvironment.WebBrowser);
                }

                WebEnvironmentRepo webEnvironmentRepo = new(uow);
                await webEnvironmentRepo.DeleteAsync(WebEnvironment);

                if (!string.IsNullOrWhiteSpace(WebEnvironment.WebBrowserDataPath) && Directory.Exists(WebEnvironment.WebBrowserDataPath))
                {
                    Directory.Delete(WebEnvironment.WebBrowserDataPath, true);
                }

                uow.Commit();

                RaiseDeleteClickEvent();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
            }
        }

        private void Button_EditWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
            if (WebEnvironment == null)
            {
                return;
            }

            new WebEnvironmentOptionWindow()
            {
                Owner = Application.Current.MainWindow,
                WebEnvironment = WebEnvironment
            }.ShowDialog();
        }
    }
}
