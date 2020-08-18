using System;
using Console.Unity.Components;
using UnityEngine;

namespace Console.Unity
{
    /// <summary>
    /// A class which will "read" the default console log and write it to the custom console
    /// </summary>
    public class DefaultLogReader : IDisposable
    {
        /// <summary>
        /// Public Constructor
        /// </summary>
		public DefaultLogReader()
        {
            Application.logMessageReceivedThreaded += HandleLog;
        }

        /// <summary>
        /// Unregisters this LogReader from the Message Received Event
        /// </summary>
        public void Dispose()
        {
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        /// <summary>
        /// Handles incoming logs.
        /// </summary>
        /// <param name="logString">The Log to Write</param>
        /// <param name="stackTrace">Stacktrace of the Log</param>
        /// <param name="logType">The LogType</param>
        private static void HandleLog(string logString, string stackTrace, LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    Log(logString);
                    break;
                case LogType.Assert:
                case LogType.Warning:
                    LogWarning(logString);
                    break;
                case LogType.Error:
                case LogType.Exception:
                    LogException(logString, stackTrace);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
            }
        }

        /// <summary>
        /// Writes a log to the custom console
        /// </summary>
        /// <param name="logString">The Log to write</param>
        private static void Log(string logString)
        {
            ConsoleManagerComponent.Log(logString, false);
        }



        /// <summary>
        /// Writes a log warning to the custom console
        /// </summary>
        /// <param name="logString">The Log warning to write</param>
        private static void LogWarning(string logString)
        {
            ConsoleManagerComponent.LogWarning(logString, false);
        }



        /// <summary>
        /// Writes a log error to the custom console
        /// </summary>
        /// <param name="logString">The Log error to write</param>
        private static void LogError(string logString)
        {
            ConsoleManagerComponent.LogError(logString, false);
        }



        /// <summary>
        /// Writes an exception to the custom console
        /// </summary>
        /// <param name="logString">The Exception to write</param>
        /// <param name="stackTrace">The Stacktrace that led to the exception</param>
        private static void LogException(string logString, string stackTrace)
        {
            string exception = $"{logString}\nStackTrace:\n{stackTrace}";

            ConsoleManagerComponent.LogError(exception, false);
        }
    }
}