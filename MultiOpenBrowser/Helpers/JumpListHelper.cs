using MultiOpenBrowser.Core.WebBrowsers;
using System.IO;
using System.Windows;
using System.Windows.Shell;
using static MultiOpenBrowser.Core.Entitys.WebBrowser;

namespace MultiOpenBrowser.Helpers
{
    public static class JumpListHelper
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
                    var arguments = new WebBrowserFactory(webEnv).GetStartupArguments(new IWebBrowser.StartOption());

                    JumpTask task = new();
                    task.Title = webEnv.Name;

                    if (webEnv.WebBrowser.Type == TypeEnum.MsEdge)
                    {
                        task.Arguments = arguments;
                        task.IconResourcePath = GlobalData.MsEdgePath;
                        task.ApplicationPath = GlobalData.MsEdgePath;
                    }
                    else if (webEnv.WebBrowser.Type == TypeEnum.Chrome)
                    {
                        task.Arguments = arguments;
                        task.IconResourcePath = GlobalData.ChromePath;
                        task.ApplicationPath = GlobalData.ChromePath;
                    }
                    else
                    {
                        task.Arguments = $"{ArgsHelper.Start_Web_Environment}={webEnv.Id}";
                        task.IconResourcePath = Environment.ProcessPath;
                        task.ApplicationPath = Environment.ProcessPath;
                        task.WorkingDirectory = Directory.GetCurrentDirectory();
                    }

                    jumpList.JumpItems.Add(task);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            JumpTask taskChrome = new()
            {
                Title = "Google Chrome",
                IconResourcePath = GlobalData.ChromePath,
                ApplicationPath = GlobalData.ChromePath,
            };
            jumpList.JumpItems.Add(taskChrome);

            JumpTask taskEdge = new()
            {
                Title = "Microsoft Edge",
                IconResourcePath = GlobalData.MsEdgePath,
                ApplicationPath = GlobalData.MsEdgePath,
            };
            jumpList.JumpItems.Add(taskEdge);

            JumpList.SetJumpList(Application.Current, jumpList);
        }
    }
}
