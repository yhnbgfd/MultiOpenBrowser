using System.Reflection;

namespace EShopHelper.Base
{
    internal static class GlobalData
    {
        internal static string? AppVersion { get; private set; }
        internal static string ChromePath { get; set; } = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        internal static string MsEdgePath { get; set; } = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        internal static Option Option { get; set; }
        internal static UserInfo? UserInfo { get; set; }
        internal static List<WebEnvironment> WebEnvironmentList { get; set; } = [];

        static GlobalData()
        {
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            Option = CacheRepo.Get<Option>("Option") ?? new();
        }
    }
}
