using System.ComponentModel;

namespace MultiOpenBrowser.Entitys
{
    [Table(Name = nameof(UserInfo))]
    public class UserInfo : INotifyPropertyChanged
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MachineCode { get; set; } = string.Empty;
        public DateTimeOffset Expired { get; set; } = DateTimeOffset.MinValue;


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
