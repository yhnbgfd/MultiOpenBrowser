namespace MultiOpenBrowser.ViewModels
{
    public class WebBrowserOptionViewModel : ReactiveObject
    {
        public WebBrowser.TypeEnum[] Types { get; }
        public WebBrowser WebBrowser { get; set; }

        public WebBrowserOptionViewModel(WebBrowser webBrowser)
        {
            Types = Enum.GetValues<WebBrowser.TypeEnum>();
            WebBrowser = webBrowser;
        }
    }
}
