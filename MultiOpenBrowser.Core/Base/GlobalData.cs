using System.Reflection;

namespace MultiOpenBrowser.Core.Base
{
    public static class GlobalData
    {
        public static string? AppVersion { get; private set; }
        public static string ChromePath { get; set; } = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        public static string MsEdgePath { get; set; } = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public static Option Option { get; set; }
        public static UserInfo? UserInfo { get; set; }
        public static List<WebEnvironment> WebEnvironmentList { get; set; } = [];
        public static List<WebEnvironmentGroup> WebEnvironmentGroupList { get; set; } = [];

        static GlobalData()
        {
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            Option = CacheRepo.Get<Option>("Option") ?? new();
        }
    }
}
