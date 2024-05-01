using System.ComponentModel;

namespace EShopHelper.Entitys
{
    [Table(Name = nameof(UserInfo))]
    public class UserInfo : INotifyPropertyChanged
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
