namespace MultiBrowserEnvTool.Base
{
    internal static class EventBus
    {
        public static Func<Task>? NotifyWebEnvironmentChange { get; set; }
    }
}
