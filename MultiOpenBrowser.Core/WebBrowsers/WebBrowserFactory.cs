using static MultiOpenBrowser.Core.Entitys.WebBrowser;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    public class WebBrowserFactory
    {
        public static (TypeEnum? type, string? arguments) GetArguments(WebEnvironment webEnvironment, StartOption startOption)
        {
            string? startResult;

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                startResult = msEdge.GetStartupArguments(startOption);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.Other)
            {
                CustomizeBrowser customizeBrowser = new(webEnvironment);
                startResult = customizeBrowser.GetStartupArguments(startOption);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                startResult = chrome.GetStartupArguments(startOption);
            }

            return (webEnvironment.WebBrowser.Type, startResult);
        }

        public static string? GetStartupCmd(WebEnvironment webEnvironment, StartOption startOption)
        {
            string? exePath;
            string? aguments;

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                exePath = webEnvironment.WebBrowser.ExePath ?? GlobalData.MsEdgePath;
                aguments = msEdge.GetStartupArguments(startOption);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.Other)
            {
                CustomizeBrowser customizeBrowser = new(webEnvironment);
                exePath = webEnvironment.WebBrowser.ExePath;
                aguments = customizeBrowser.GetStartupArguments(startOption);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                exePath = webEnvironment.WebBrowser.ExePath ?? GlobalData.ChromePath;
                aguments = chrome.GetStartupArguments(startOption);
            }

            return $"{exePath} {aguments}";
        }

        public static void Start(WebEnvironment webEnvironment, StartOption startOption)
        {
            StartResult startResult;

            if (webEnvironment.WebBrowser.Type == TypeEnum.MsEdge)
            {
                MsEdge msEdge = new(webEnvironment);
                startResult = msEdge.Start(startOption);
            }
            else if (webEnvironment.WebBrowser.Type == TypeEnum.Other)
            {
                CustomizeBrowser customizeBrowser = new(webEnvironment);
                startResult = customizeBrowser.Start(startOption);
            }
            else
            {
                Chrome chrome = new(webEnvironment);
                startResult = chrome.Start(startOption);
            }

            if (startResult.IsSuccess == true)
            {
                webEnvironment.ProcessId = startResult.ProcessId;
            }
        }
    }
}
