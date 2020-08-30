namespace Console.Core.LogSystem
{
    /// <summary>
    /// Logger that can Prepend a Prefix to the Logs
    /// </summary>
    public class PrefixLogger : ALogger
    {
        /// <summary>
        /// The Prefix that is used for this Logger
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Public Constructor
        /// </summary>
        public PrefixLogger()
        {
            Prefix = "";
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="prefix">Prefix</param>
        public PrefixLogger(string prefix)
        {
            Prefix = $"[{prefix}]";
        }

        /// <summary>
        /// Writes a Log to the Console
        /// </summary>
        /// <param name="value">Object to write</param>
        public override void Log(object value)
        {
            base.Log(Prefix + value);
        }

        /// <summary>
        /// Writes a Log Warning to the Console
        /// </summary>
        /// <param name="value">Object to write</param>
        public override void LogWarning(object value)
        {
            base.LogWarning(Prefix + value);
        }

        /// <summary>
        /// Writes a Log Error to the Console
        /// </summary>
        /// <param name="value">Object to write</param>
        public override void LogError(object value)
        {
            base.LogError(Prefix + value);
        }
    }
}