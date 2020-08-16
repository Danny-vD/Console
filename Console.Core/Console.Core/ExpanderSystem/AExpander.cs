using Console.Core.ActivationSystem;

namespace Console.Core.ExpanderSystem
{
    /// <summary>
    /// Expander Class, used to implement Custom Expanders that expand the input string before the parsing step
    /// </summary>
    [ActivateOn]
    public abstract class AExpander
    {
        /// <summary>
        /// Returns the Expanded String based on the Input String
        /// </summary>
        /// <param name="input">Input String</param>
        /// <returns>Expanded String</returns>
        public abstract string Expand(string input);
    }
}