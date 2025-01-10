using System.Windows;
using System.Windows.Controls;
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (WebBrowser.TypeEnum type in Enum.GetValues<WebBrowser.TypeEnum>())
            {
                this.ComboBox_Type.Items.Add(type.ToString());
            }
            this.ComboBox_Type.SelectedItem = WebBrowser.Type.ToString();
        }

        private void ComboBox_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string? text = ((sender as ComboBox)?.SelectedItem as ComboBoxItem)?.Content as string;
            string? text = (sender as ComboBox)?.SelectedItem as string;
            if (Enum.TryParse(text, out WebBrowser.TypeEnum type))
            {
                WebBrowser.Type = type;
            }
            else
            {
                WebBrowser.Type = WebBrowser.TypeEnum.Chrome;
            }
        }
    }
}
