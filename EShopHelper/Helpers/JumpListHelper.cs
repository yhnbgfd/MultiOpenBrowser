using System.IO;
using System.Windows;
using System.Windows.Shell;

namespace EShopHelper.Helpers
{
    internal static class JumpListHelper
    {
        internal static void SetJumpList()
        {
            JumpList jumpList = new()
            {
                ShowFrequentCategory = false,
                ShowRecentCategory = false
            };

            foreach (var item in GlobalData.WebEnvironmentList)
            {
                JumpTask task = new()
                {
                    Title = item.Name,
                    Arguments = $"--start-web-environment={item.Id}",
                    Description = item.Name,
                    //CustomCategory = "WebEnvironments",
                    IconResourcePath = Environment.ProcessPath,
                    ApplicationPath = Environment.ProcessPath,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                };
                jumpList.JumpItems.Add(task);
            }

            JumpTask taskChrome = new()
            {
                Title = "Google Chrome",
                Description = "Google Chrome",
                //CustomCategory = "System",
                IconResourcePath = GlobalData.ChromePath,
                ApplicationPath = GlobalData.ChromePath,
            };
            jumpList.JumpItems.Add(taskChrome);

            JumpList.SetJumpList(Application.Current, jumpList);
        }
    }
}
