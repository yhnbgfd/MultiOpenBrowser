namespace EShopHelper.Base
{
    internal static class GlobalData
    {
        internal static string ChromePath { get; set; } = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        internal static Option Option { get; set; }
        internal static UserInfo? UserInfo { get; set; }
        internal static List<WebEnvironment> WebEnvironmentList { get; set; } = [];

        static GlobalData()
        {
            Option = CacheRepo.Get<Option>("Option") ?? new();
        }
    }
}
