namespace MultiOpenBrowser.Entitys
{
    [Table(Name = nameof(WebEnvironmentGroup))]
    public class WebEnvironmentGroup
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Order { get; set; }
    }
}
