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
                    CustomCategory = "WebEnvironment List",
                    IconResourcePath = Environment.ProcessPath,
                    ApplicationPath = Environment.ProcessPath,
                    WorkingDirectory = Directory.GetCurrentDirectory(),
                };
                jumpList.JumpItems.Add(task);
            }

            JumpList.SetJumpList(Application.Current, jumpList);
        }
    }
}
