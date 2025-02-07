using Autofac;
using System.Runtime.Versioning;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    public class WebBrowserFactory(WebEnvironment webEnvironment) : WebBrowserBase(webEnvironment)
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WebBrowserBase WebBrowserInstance => Global.Container.ResolveKeyed<WebBrowserBase>(_webEnvironment.WebBrowser.Type, new NamedParameter("webEnvironment", _webEnvironment));

        public override string? GetStartupArguments(StartOption startOption)
        {
            var startResult = WebBrowserInstance.GetStartupArguments(startOption);
            return startResult;
        }

        public override string? GetStartupCmd(StartOption startOption)
        {
            string? exePath = WebBrowserInstance.ExePath;
            var aguments = WebBrowserInstance.GetStartupArguments(startOption);
            return $"{exePath} {aguments}";
        }

        public override StartResult Start(StartOption startOption)
        {
            var startResult = WebBrowserInstance.Start(startOption);
            if (startResult.IsSuccess == true)
            {
                _webEnvironment.ProcessId = startResult.ProcessId;
            }
            return startResult;
        }

        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        /// <param name="startOption">启动选项</param>
        /// <returns>是否创建成功</returns>
        [SupportedOSPlatform("windows")]
        public bool CreateShortcut(StartOption startOption)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string shortcutPath = Path.Combine(desktopPath, $"{_webEnvironment.Name}.lnk");

                // 创建 Shell 对象
                var shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null)
                {
                    return false;
                }
                dynamic? shell = Activator.CreateInstance(shellType);
                if (shell == null)
                {
                    return false;
                }

                // 创建快捷方式对象
                var shortcut = shell.CreateShortcut(shortcutPath);

                // 设置目标路径和参数
                shortcut.TargetPath = WebBrowserInstance.ExePath;  // 可执行文件路径
                shortcut.Arguments = WebBrowserInstance.GetStartupArguments(startOption);  // 启动参数

                // 设置图标
                shortcut.IconLocation = WebBrowserInstance.ExePath;

                // 保存快捷方式
                shortcut.Save();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
    }
}
