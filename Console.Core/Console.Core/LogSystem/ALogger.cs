using System;

namespace Console.Core.LogSystem
{
    public abstract class ALogger
    {
        public bool Mute { get; set; }
        public virtual void Log(object value)
        {
            if (!Mute)
                AConsoleManager.Instance._Log(value);
        }

        public virtual void LogWarning(object value)
        {
            if (!Mute)
                AConsoleManager.Instance._LogWarning(value);
        }

        public virtual void LogError(object value)
        {
            if (!Mute)
                AConsoleManager.Instance._LogError(value);
        }
    }

    public class DefaultLogger : ALogger { }

    public class PrefixLogger : ALogger
    {
        public string Prefix { get; set; }

        public PrefixLogger()
        {
            Prefix = "";
        }

        public PrefixLogger(string prefix)
        {
            Prefix = $"[{prefix}] ";
        }

        public override void Log(object value)
        {
            base.Log(Prefix + value);
        }
        public override void LogWarning(object value)
        {
            base.LogWarning(Prefix + "Warning: " + value);
        }
        public override void LogError(object value)
        {
            base.LogError(Prefix + "Error: " + value);
        }
    }
}