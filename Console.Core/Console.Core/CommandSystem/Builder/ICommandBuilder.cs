using System;

namespace Console.Core.CommandSystem.Builder
{
    /// <summary>
    /// Command Builder Interface
    /// </summary>
    public interface ICommandBuilder
    {

        /// <summary>
        /// If true the Command was Built.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// The Cursor Position
        /// </summary>
        int Cursor { get; }

        /// <summary>
        /// Clears the Command Builder
        /// </summary>
        void Clear();


        /// <summary>
        /// Handles the Input From the Console
        /// </summary>
        /// <param name="info">Key Info</param>
        void Input(ConsoleKeyInfo info);

        /// <summary>
        /// Returns the Command String
        /// </summary>
        /// <returns>Command String</returns>
        string ToString();

    }
}