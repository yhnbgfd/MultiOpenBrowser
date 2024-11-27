namespace MultiOpenBrowser.Core.Entitys
{
    [Table(Name = nameof(WebEnvironmentGroup))]
    public class WebEnvironmentGroup : INotifyPropertyChanged, ICloneable
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Order { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
