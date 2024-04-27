namespace EShopHelper.Entitys
{
    [Table(Name = nameof(Cache))]
    public class Cache
    {
        [Column(IsPrimary = true)]
        public string Key { get; set; } = string.Empty;
        public string? Value { get; set; }
        public DateTimeOffset? Expired { get; set; }

    }
}
