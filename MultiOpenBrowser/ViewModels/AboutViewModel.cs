namespace MultiOpenBrowser.ViewModels
{
    public class AboutViewModel : ReactiveObject
    {
        public string? AppVersion => GlobalData.AppVersion;
    }
}
