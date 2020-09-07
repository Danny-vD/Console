using System;

namespace Console.Core.CommandSystem.Builder
{
    /// <summary>
    /// Abstaction that enables compatibility with multiple input types.
    /// </summary>
    public interface ICommandBuilderInput
    {

        /// <summary>
        /// Gets or Sets the Value that indicates that the Current Command Build should be aborted
        /// </summary>
        bool Abort { get; set; }

        /// <summary>
        /// Blocking Call.
        /// Does Wait until the User has Pressed a Key
        /// </summary>
        /// <returns>The Pressed Key Info</returns>
        ConsoleKeyInfo ReadKey();

        /// <summary>
        /// Writes a String to the Input
        /// </summary>
        /// <param name="str"></param>
        void Write(string str);

        /// <summary>
        /// Deletes all Content of the Line and Sets the Cursor Position to the Beginning of the Line
        /// </summary>
        void ResetLine();

        /// <summary>
        /// Ends the Line
        /// </summary>
        void EndLine();

        /// <summary>
        /// Sets the Cursor Position in the InputField
        /// </summary>
        /// <param name="pos">New Cursor Position</param>
        void SetCursorPosition(int pos);

    }
}