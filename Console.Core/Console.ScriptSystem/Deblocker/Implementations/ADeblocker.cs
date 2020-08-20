using System.Collections.Generic;
using Console.Core.ActivationSystem;


/// <summary>
/// ADeblocker Implementations.
/// </summary>
namespace Console.ScriptSystem.Deblocker.Implementations
{
    /// <summary>
    /// ADeblocker class. Used to Customize the way a Line gets Deblocked.
    /// </summary>
    [ActivateOn]
    public abstract class ADeblocker
    {
        /// <summary>
        /// The Key of the Deblocker that has to match the block command to be activated.
        /// </summary>
        public abstract string Key { get; }
        /// <summary>
        /// Returns the Deblocked Content of the Line
        /// </summary>
        /// <param name="line">Line to Deblock</param>
        /// <param name="begin">Lines that get prepended to the beginning of the file</param>
        /// <param name="end">Lines that get appended to the end of the file</param>
        /// <returns>List of Deblocked Content</returns>
        public abstract string[] Deblock(Line line,out List<string> begin, out List<string> end);
    }
}