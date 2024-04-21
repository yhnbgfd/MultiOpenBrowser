using NLog;
using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; }

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
                WebEnvironment.StartWebBrowser();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async void Button_DeleteWebEnvironment_Click(object sender, RoutedEventArgs e)
        {
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

                uow.Commit();

                //TODO 删除 WebEnvironment.WebBrowserDataPath 文件夹

                RaiseDeleteClickEvent();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
            }
        }
    }
}
