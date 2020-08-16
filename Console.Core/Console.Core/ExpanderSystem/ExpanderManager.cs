using System.Collections.Generic;

namespace Console.Core.ExpanderSystem
{

    /// <summary>
    /// Expander System API.
    /// </summary>
    public class ExpanderManager
    {
        /// <summary>
        /// List of Loaded Expanders
        /// </summary>
        private readonly List<AExpander> Expanders = new List<AExpander>();

        /// <summary>
        /// Adds an Expander to the Loaded Expanders
        /// </summary>
        /// <param name="expander">Expander to Add</param>
        public void AddExpander(AExpander expander)
        {
            Expanders.Add(expander);
        }

        /// <summary>
        /// Expands the Specified String with the Loaded Expanders.
        /// </summary>
        /// <param name="input">Input String</param>
        /// <returns>Expanded String</returns>
        public string Expand(string input)
        {
            string ret = input;
            for (int i = 0; i < Expanders.Count; i++)
            {
                ret = Expanders[i].Expand(ret);
            }
            return ret;
        }
    }
}