using System;

using Console.Core.CommandSystem.Builder;

namespace Console.CLI
{
    /// <summary>
    /// Abstaction that enables compatibility with the CLI Console
    /// </summary>
    public class ConsoleBuilderInput : ICommandBuilderInput
    {

        /// <summary>
        /// Gets or Sets the Value that indicates that the Current Command Build should be aborted
        /// </summary>
        public bool Abort { get; set; }

        /// <summary>
        /// Writes a String to the Input
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            System.Console.Write(str);
        }

        /// <summary>
        /// Blocking Call.
        /// Does Wait until the User has Pressed a Key
        /// </summary>
        /// <returns>The Pressed Key Info</returns>
        public ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }


        /// <summary>
        /// Ends the Line
        /// </summary>
        public void EndLine()
        {
            System.Console.WriteLine();
        }


        /// <summary>
        /// Sets the Cursor Position in the InputField
        /// </summary>
        /// <param name="pos">New Cursor Position</param>
        public void SetCursorPosition(int pos)
        {
            System.Console.CursorLeft = pos;
        }


        /// <summary>
        /// Deletes all Content of the Line and Sets the Cursor Position to the Beginning of the Line
        /// </summary>
        public void ResetLine()
        {
            System.Console.CursorLeft = 0;
            for (int i = 0; i < System.Console.WindowWidth - 1; i++)
            {
                System.Console.Write(' ');
            }

            System.Console.CursorLeft = 0;
        }

    }
}