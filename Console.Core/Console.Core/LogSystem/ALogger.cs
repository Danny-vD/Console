using System;


/// <summary>
/// The Log System provides Logger Implementations that allow for prefixes and other transformations of the incoming logs
/// </summary>
namespace Console.Core.LogSystem
{
    /// <summary>
    /// ALogger is an Abstraction to be able to have custom Logger Prefixes.
    /// </summary>
    public abstract class ALogger
    {
        /// <summary>
        /// If true the Logger will not output any Logs
        /// </summary>
        public bool Mute { get; set; }

        private ALogger Sub;

        public void WrapAround(ALogger logger)
        {
            ALogger sub = this;
            while (sub.Sub != null)
            {
                sub = sub.Sub;
            }
            sub.Sub = logger;
        }

        /// <summary>
        /// Writes a Log to the Console
        /// </summary>
        /// <param name="value">Object to write</param>
        public virtual void Log(object value)
        {
            if (!Mute)
            {
                if (Sub == null)
                    AConsoleManager.Instance._Log(value);
                else Sub.Log(value);
            }
        }


        /// <summary>
        /// Writes a Log Warning to the Console
        /// </summary>
        /// <param name="value">Object to write</param>
        public virtual void LogWarning(object value)
        {
            if (!Mute)
            {
                if (Sub == null)
                    AConsoleManager.Instance._LogWarning(value);
                else Sub.Log(value);
            }
        }


        /// <summary>
        /// Writes a Log Error to the Console
        /// </summary>
        /// <param name="value">Object to write</param>
        public virtual void LogError(object value)
        {
            if (!Mute)
            {
                if (Sub == null)
                    AConsoleManager.Instance._LogError(value);
                else Sub.Log(value);
            }
        }


    }
}