namespace Console.Core.LogSystem
{
    /// <summary>
    /// The TypedLoggger Implementation is Prepending Prefixes for each Log Type(Log/Warning/Error)
    /// </summary>
    public class TypedLogger : ALogger
    {

        /// <summary>
        /// The Prefix that is used by normal Logs
        /// </summary>
        public string LogPrefix { get; set; } = "[Log]";

        /// <summary>
        /// The Prefix that is used by warning logs
        /// </summary>
        public string LogWarningPrefix { get; set; } = "[Warning]";

        /// <summary>
        /// The Prefix that is used by error logs
        /// </summary>
        public string LogErrorPrefix { get; set; } = "[Error]";

        /// <summary>
        /// Logs an Object
        /// </summary>
        /// <param name="value">Object to log</param>
        public override void Log(object value)
        {
            base.Log(LogPrefix + value);
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="value">Object to log</param>
        public override void LogWarning(object value)
        {
            base.LogWarning(LogWarningPrefix + value);
        }

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="value">Object to log</param>
        public override void LogError(object value)
        {
            base.LogError(LogErrorPrefix + value);
        }

        /// <summary>
        /// Creates a Typed Logger that displays the Prefix and the Log Types.
        /// </summary>
        /// <param name="prefix">Prefix to Use</param>
        /// <param name="logPrefixFirst">If true will write [Log][Prefix]LogMessage, If false will write [Prefix][Log]LogMessage</param>
        /// <returns></returns>
        public static ALogger CreateTypedWithPrefix(string prefix, bool logPrefixFirst = true)
        {
            if (logPrefixFirst)
            {
                ALogger log = new PrefixLogger(prefix);
                log.WrapAround(new TypedLogger());
                return log;
            }

            TypedLogger pfx = new TypedLogger();
            pfx.WrapAround(new PrefixLogger(prefix));
            return pfx;
        }

    }
}