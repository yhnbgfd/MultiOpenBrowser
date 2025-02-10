using MultiOpenBrowser.Core.WebBrowsers;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MultiOpenBrowser.Views.UserControls
{
    public partial class WebEnvironmentListItemUserControl : ReactiveUserControl<WebEnvironmentListItemViewModel>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const double IconOpacityDefault = 0.1;
        private const double IconOpacityFocus = 0.2;
        public WebEnvironment WebEnvironment { get; private set; }

        public WebEnvironmentListItemUserControl(WebEnvironment webEnvironment)
        {
            InitializeComponent();
            WebEnvironment = webEnvironment;

            this.WhenActivated(disposables =>
            {
                ViewModel = new WebEnvironmentListItemViewModel(webEnvironment);
                this.OneWayBind(ViewModel, vm => vm.WebEnvironment.NameUI, v => v.TextBlock_Name.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.WebEnvironment.ToolTip, v => v.TextBlock_Name.ToolTip).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.StartWebEnvironmentCommand, x => x.Button_Run).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.StartWebEnvironmentIncognitoCommand, x => x.MenuItem_StartWebEnvironmentIncognito).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.EditWebEnvironmentCommand, x => x.MenuItem_EditWebEnvironment).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.CopyWebEnvironmentCommand, x => x.MenuItem_Copy).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.OpenDataFolderCommand, x => x.MenuItem_OpenDataFolder).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.DeleteWebEnvironmentCommand, x => x.MenuItem_DeleteWebEnvironment).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.CopyStartupCMDCommand, x => x.MenuItem_CopyStartupCMD).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.CopyDataFolderPathCommand, x => x.MenuItem_CopyDataFolderPath).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.CreateShortcutCommand, x => x.MenuItem_CreateShortcut).DisposeWith(disposables);
            });

            WebBrowserFactory webBrowserFactory = new(webEnvironment);
            var iconName = webBrowserFactory.WebBrowserInstance.Icon;
            var iconPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", iconName);
            Uri uriSource = new(iconPath);
            this.Image_Icon.Source = new BitmapImage(uriSource);
            this.Image_Icon.Opacity = IconOpacityDefault;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Image_Icon.Opacity = IconOpacityFocus;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Image_Icon.Opacity = IconOpacityDefault;
        }
    }
}
