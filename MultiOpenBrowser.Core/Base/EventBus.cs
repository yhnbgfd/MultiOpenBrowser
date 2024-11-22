namespace MultiOpenBrowser.Core.Base
{
    public static class EventBus
    {
        public static Func<Task>? LockUI { get; set; }
        public static Func<Task>? UnlockUI { get; set; }
        public static Func<Task>? NotifyWebEnvironmentChange { get; set; }
    }
}
