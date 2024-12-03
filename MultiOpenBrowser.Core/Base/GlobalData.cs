using System.Reflection;

namespace MultiOpenBrowser.Core.Base
{
    public static class GlobalData
    {
        public static string? AppVersion { get; private set; }
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
