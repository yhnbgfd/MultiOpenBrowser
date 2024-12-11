using System.Text;
using static MultiOpenBrowser.Core.WebBrowsers.IWebBrowser;

namespace MultiOpenBrowser.Core.WebBrowsers
{
    public abstract class WebBrowserBase(WebEnvironment webEnvironment) : IWebBrowser
    {
        protected WebEnvironment _webEnvironment = webEnvironment;

        protected virtual string ArgumentPrefix => "--";

        public abstract string? GetStartupArguments(StartOption startOption);
        public abstract string? GetStartupCmd(StartOption startOption);
        public abstract StartResult Start(StartOption startOption);

        protected void AppendArgument(StringBuilder sb, string name, string? value = null)
        {
            if (value != null)
            {
                sb.Append($"{ArgumentPrefix}{name}=\"{value}\" ");
            }
            else
            {
                sb.Append($"{ArgumentPrefix}{name} ");
            }
        }
    }
}
