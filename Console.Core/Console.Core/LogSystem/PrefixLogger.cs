namespace Console.Core.LogSystem
{
    /// <summary>
    /// Logger that can Prepend a Prefix to the Logs
    /// </summary>
    public class PrefixLogger : ALogger
    {
        public string Prefix { get; set; }

        public PrefixLogger()
        {
            Prefix = "";
        }

        public PrefixLogger(string prefix)
        {
            Prefix = $"[{prefix}]";
        }

        public override void Log(object value)
        {
            base.Log(Prefix + value);
        }

        public override void LogWarning(object value)
        {
            base.LogWarning(Prefix + value);
        }

        public override void LogError(object value)
        {
            base.LogError(Prefix + value);
        }
    }
}