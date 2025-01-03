﻿using MultiOpenBrowser.Core.WebBrowsers;

namespace MultiOpenBrowser.Core.Helpers
{
    public class ArgsHelper
    {
        public const string Start_Web_Environment = "--start-web-environment";

        public static string? GetArgsValue(string key, params string[] args)
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
        public static async Task StartWebEnvironmentAsync(params string[] args)
        {
            var startWebEnvironmenArg = GetArgsValue(Start_Web_Environment, args);
            if (startWebEnvironmenArg != null)
            {
                if (int.TryParse(startWebEnvironmenArg, out var id))
                {
                    var webEnvironment = await new WebEnvironmentRepo(null).GetAsync(id);
                    if (webEnvironment != null)
                    {
                        new WebBrowserFactory(webEnvironment).Start(new IWebBrowser.StartOption());
                        //Environment.Exit(0);
                    }
                }
            }
        }
    }
}
