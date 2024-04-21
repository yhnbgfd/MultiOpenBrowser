using NLog;
using System.Windows;
using System.Windows.Controls;

namespace EShopHelper.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : UserControl
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; }

        public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent(
            name: "Delete",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(WebEnvironmentListItemUserControl));

        public event RoutedEventHandler Delete
        {
            add { AddHandler(DeleteEvent, value); }
            remove { RemoveHandler(DeleteEvent, value); }
        }

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            DataContext = this;
            WebEnvironment = webEnvironment;
        }

        void RaiseDeleteEvent()
        {
            RoutedEventArgs routedEventArgs = new(routedEvent: DeleteEvent);
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

                RaiseDeleteEvent();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
            }
        }
    }
}
