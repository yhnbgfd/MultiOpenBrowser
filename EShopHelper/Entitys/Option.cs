using System.ComponentModel;

namespace EShopHelper.Entitys
{
    public class Option : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 默认浏览器数据文件夹
        /// </summary>
        public string DefaultWebBrowserDataPath { get; set; } = "";
    }
}
