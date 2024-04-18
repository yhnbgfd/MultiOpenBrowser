using System.Reflection;

namespace EShopHelper.Entitys
{
    [Table(Name = nameof(UserInfo))]
    [Obfuscation(Exclude = true)]
    public class UserInfo
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
