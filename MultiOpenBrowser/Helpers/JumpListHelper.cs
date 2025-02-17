using MultiOpenBrowser.Core.WebBrowsers;
using System.IO;
using System.Windows;
using System.Windows.Shell;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Helpers
{
    internal static class JumpListHelper
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void SetJumpList()
        {
            JumpList jumpList = new()
            {
                ShowFrequentCategory = false,
                ShowRecentCategory = false
            };

            foreach (var webEnv in GlobalData.WebEnvironmentList)
            {
                try
                {
                    if (webEnv.ShowInJumpList == false)
                    {
                        continue;
                    }

                    WebBrowserFactory webBrowserFactory = new(webEnv);
                    if (!File.Exists(webBrowserFactory.WebBrowserInstance.ExePath))
                    {
                        continue;
                    }

                    JumpTask task = new()
                    {
                        Title = webEnv.Name,
                        Description = webEnv.Name,
                        Arguments = webBrowserFactory.GetStartupArguments(StartOption.Default),
                        ApplicationPath = webBrowserFactory.WebBrowserInstance.ExePath,
                        IconResourcePath = webBrowserFactory.WebBrowserInstance.ExePath,
                    };

                    jumpList.JumpItems.Add(task);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            JumpTask taskChrome = new()
            {
                Title = "SYS: Google Chrome",
                IconResourcePath = GlobalData.Option.ChromePath,
                ApplicationPath = GlobalData.Option.ChromePath,
            };
            jumpList.JumpItems.Add(taskChrome);

            JumpTask taskEdge = new()
            {
                Title = "SYS: Microsoft Edge",
                IconResourcePath = GlobalData.Option.MsEdgePath,
                ApplicationPath = GlobalData.Option.MsEdgePath,
            };
            jumpList.JumpItems.Add(taskEdge);

            JumpList.SetJumpList(Application.Current, jumpList);
        }
    }
}
