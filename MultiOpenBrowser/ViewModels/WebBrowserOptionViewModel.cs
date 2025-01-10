namespace MultiOpenBrowser.ViewModels
{
    public class WebBrowserOptionViewModel : ReactiveObject
    {
        public List<string> Types { get; }
        public WebBrowser WebBrowser { get; set; }

        public WebBrowserOptionViewModel(WebBrowser webBrowser)
        {
            Types = new(Enum.GetNames<WebBrowser.TypeEnum>());
            WebBrowser = webBrowser;
        }
    }
}
