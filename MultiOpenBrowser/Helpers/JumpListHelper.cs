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

            foreach (var item in GlobalData.WebEnvironmentList)
            {
                var (type, arguments) = WebBrowserFactory.GetArguments(item, new IWebBrowser.StartOption());

                JumpTask task = new();
                task.Title = item.Name;

                if (type == TypeEnum.MsEdge)
                {
                    task.Arguments = arguments;
                    task.IconResourcePath = GlobalData.MsEdgePath;
                    task.ApplicationPath = GlobalData.MsEdgePath;
                }
                else if (type == TypeEnum.Chrome)
                {
                    task.Arguments = arguments;
                    task.IconResourcePath = GlobalData.ChromePath;
                    task.ApplicationPath = GlobalData.ChromePath;
                }
                else
                {
                    task.Arguments = $"{ArgsHelper.Start_Web_Environment}={item.Id}";
                    task.IconResourcePath = Environment.ProcessPath;
                    task.ApplicationPath = Environment.ProcessPath;
                    task.WorkingDirectory = Directory.GetCurrentDirectory();
                }

                jumpList.JumpItems.Add(task);
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
