using System.Windows;
using WebBrowser = MultiOpenBrowser.Core.Entitys.WebBrowser;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebBrowserOptionUserControl : ReactiveUserControl<WebBrowserOptionViewModel>
    {
        public static readonly DependencyProperty WebBrowserProperty = DependencyProperty.Register(
            "WebBrowser", typeof(WebBrowser), typeof(WebBrowserOptionUserControl), new PropertyMetadata(new WebBrowser() { Name = "WebBrowser" }));

        public WebBrowser WebBrowser
        {
            get { return (WebBrowser)GetValue(WebBrowserProperty); }
            set { SetValue(WebBrowserProperty, value); }
        }

        public WebBrowserOptionUserControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ViewModel = new WebBrowserOptionViewModel(WebBrowser);
                this.OneWayBind(ViewModel, vm => vm.Types, v => v.ComboBox_Type.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.Type, v => v.ComboBox_Type.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.ExePath, v => v.TextBox_ExePath.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.Arguments, v => v.TextBox_Arguments.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.Name, v => v.TextBox_WebBrowserName.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.UserAgent, v => v.TextBox_UserAgent.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.ProxyServer, v => v.TextBox_ProxyServer.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.WebBrowser.DisableWebSecurity, v => v.CheckBox_DisableWebSecurity.IsChecked).DisposeWith(disposables);
                this.ComboBox_Type.SelectedItem = WebBrowser.Type;
            });
        }
    }
}
