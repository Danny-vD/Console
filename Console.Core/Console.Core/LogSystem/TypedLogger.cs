namespace Console.Core.LogSystem
{
    public class TypedLogger : ALogger
    {
        public string LogPrefix { get; set; } = "[Log]";
        public string LogWarningPrefix { get; set; } = "[Warning]";
        public string LogErrorPrefix { get; set; } = "[Error]";


        public override void Log(object value)
        {
            base.Log(LogPrefix + value);
        }
        public override void LogWarning(object value)
        {
            base.LogWarning(LogWarningPrefix + value);
        }
        public override void LogError(object value)
        {
            base.LogError(LogErrorPrefix + value);
        }

        public static ALogger CreateTypedWithPrefix(string prefix, bool logPrefixFirst = false)
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