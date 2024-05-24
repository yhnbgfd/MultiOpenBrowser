namespace EShopHelper.Helpers
{
    internal class ArgsHelper
    {
        internal const string Start_Web_Environment = "--start-web-environment";

        internal static string? GetArgsValue(string key, params string[] args)
        {
            var arg = args.FirstOrDefault(a => a.StartsWith($"{key}="));
            if (arg != null)
            {
                var argsSplit = arg.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                if (argsSplit.Length > 1)
                {
                    return argsSplit[1];
                }
            }

            return null;
        }

        /// <summary>
        /// 启动WebEnvironment
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static async Task StartWebEnvironmentAsync(params string[] args)
        {
            var startWebEnvironmenArg = GetArgsValue(Start_Web_Environment, args);
            if (startWebEnvironmenArg != null)
            {
                if (int.TryParse(startWebEnvironmenArg, out var id))
                {
                    WebEnvironmentRepo webEnvironmentRepo = new(null);
                    var webEnvironment = await webEnvironmentRepo.Select
                        .Where(a => a.Id == id)
                        .FirstAsync();
                    if (webEnvironment != null)
                    {
                        webEnvironment.StartWebBrowser();
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
