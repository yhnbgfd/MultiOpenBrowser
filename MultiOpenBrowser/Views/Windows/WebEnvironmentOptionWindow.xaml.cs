using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultiOpenBrowser.Views.Windows
{
    public partial class WebEnvironmentOptionWindow : Window, INotifyPropertyChanged
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebEnvironment WebEnvironment { get; set; } = WebEnvironment.Default;
        public WebBrowser? WebBrowser { get; set; }
        public WebEnvironmentGroup? WebEnvironmentGroup { get; set; }

        public WebEnvironmentOptionWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            {
                ComboBoxItem comboBoxItem = new()
                {
                    Content = "No Group",
                };
                this.ComboBox_Group.Items.Add(comboBoxItem);
            }
            foreach (var group in GlobalData.WebEnvironmentGroupList)
            {
                ComboBoxItem comboBoxItem = new()
                {
                    Content = group.Name,
                    Tag = group
                };
                this.ComboBox_Group.Items.Add(comboBoxItem);
            }

            WebBrowser ??= this.WebEnvironment.WebBrowser;
            WebEnvironmentGroup ??= this.WebEnvironment.WebEnvironmentGroup;

            if (WebEnvironmentGroup != null)
            {
                this.ComboBox_Group.SelectedIndex = GlobalData.WebEnvironmentGroupList.FindIndex(0, a => a.Name == WebEnvironmentGroup.Name) + 1;
            }
            else
            {
                this.ComboBox_Group.SelectedIndex = 0;
            }
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            using var uow = Global.FSql.CreateUnitOfWork();
            try
            {
                WebBrowserRepo webBrowserRepo = new(uow);
                WebEnvironment.WebBrowser = await webBrowserRepo.InsertOrUpdateAsync(WebBrowser!);
                WebEnvironment.WebBrowserId = WebEnvironment.WebBrowser!.Id;
                WebEnvironment.WebEnvironmentGroupId = WebEnvironmentGroup?.Id;

                WebEnvironmentRepo webEnvironmentRepo = new(uow);
                await webEnvironmentRepo.InsertOrUpdateAsync(WebEnvironment);

                uow.Commit();

                JumpListHelper.SetJumpList();

                EventBus.OnWebEnvironmentListChange?.Invoke();

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                _logger.Error(ex);
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Save WebEnvironment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ComboBox_Group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebEnvironmentGroup? tag = ((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Tag as WebEnvironmentGroup;
            WebEnvironmentGroup = tag;
        }
    }
}
