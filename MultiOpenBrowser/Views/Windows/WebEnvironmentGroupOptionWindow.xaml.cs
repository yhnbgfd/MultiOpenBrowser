using System.Windows;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class WebEnvironmentGroupOptionWindow : Window
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironmentGroup WebEnvironmentGroup { get; set; } = new();

        public WebEnvironmentGroupOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebEnvironmentGroupRepo repo = new(null);
                await repo.InsertOrUpdateAsync(WebEnvironmentGroup);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
