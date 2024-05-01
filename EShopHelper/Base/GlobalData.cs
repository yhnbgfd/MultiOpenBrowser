namespace EShopHelper.Base
{
    internal static class GlobalData
    {
        internal static UserInfo? UserInfo { get; set; }
        internal static List<WebEnvironment> WebEnvironmentList { get; set; } = [];
    }
}
