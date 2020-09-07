using System.Collections.Generic;

/// <summary>
/// Implements the deblocking process for the commands if/ifelse/ifelseif/for-all and while
/// </summary>
namespace Console.Script.Deblocker.Conditions
{
    /// <summary>
    /// Implements the deblocking for the "while" command.
    /// </summary>
    public class WhileDeblocker : IfDeblocker
    {

        /// <summary>
        /// Deblocker Key
        /// </summary>
        public override string Key => "while";


        /// <summary>
        /// Deblocks the while command.
        /// </summary>
        /// <param name="line">The Command</param>
        /// <param name="begin">Lines that should be prepended before the line deblock</param>
        /// <param name="end">Lines that should be appended after the line deblock</param>
        /// <returns>The Deblocked Line</returns>
        public override string[] Deblock(Line line, out List<string> begin, out List<string> end)
        {
            return Deblock(line, new string[0], out begin, out end);
        }

    }
}