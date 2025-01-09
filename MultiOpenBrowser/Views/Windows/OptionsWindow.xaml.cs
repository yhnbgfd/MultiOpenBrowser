using MultiOpenBrowser.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class OptionsWindow : ReactiveWindow<OptionsViewModel>
    {
        public OptionsWindow()
        {
            InitializeComponent();

            ViewModel = new OptionsViewModel();
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.Option.DefaultWebBrowserDataPath, v => v.TextBox_DefaultWebBrowserDataPath.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.Option.DefaultUserAgent, v => v.TextBox_DefaultUserAgent.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.SaveCommand, x => x.Button_Save).DisposeWith(disposables);
                ViewModel.SaveCommand
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(result =>
                    {
                        this.Close();
                    })
                    .DisposeWith(disposables);
            });
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
