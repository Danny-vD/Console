using System.Collections.Generic;
using System.Text;

namespace Console.ScriptSystem.Extensions
{
    /// <summary>
    /// Abstract Extension Filter that is used to allow/disallow scripts with specific extensions to be ran.
    /// </summary>
    public abstract class AExtensionFilter
    {
        /// <summary>
        /// Returns true if the extension is allowed to be loaded.
        /// </summary>
        /// <param name="extensions">Extension to Check</param>
        /// <returns>True if Allowed</returns>
        public abstract bool Allowed(string extensions);


        private static readonly List<AExtensionFilter> Filters = new List<AExtensionFilter>();

        /// <summary>
        /// Returns True if the Extension is allowed by the loaded Filters
        /// </summary>
        /// <param name="extension">The Extension to be Checked</param>
        /// <returns>True if allowed.</returns>
        public static bool IsAllowed(string extension)
        {
            bool allowed = true;
            for (int i = 0; i < Filters.Count; i++)
            {
                allowed &= Filters[i].Allowed(extension);
            }
            return allowed;
        }
        /// <summary>
        /// Adds a Filter to the Loaded Filters
        /// </summary>
        /// <param name="filter">Filter to Add</param>
        public static void AddFilter(AExtensionFilter filter)
        {
            if (!Filters.Contains(filter))
                Filters.Add(filter);
        }

        /// <summary>
        /// Removes a Filter fro the Loaded Filters
        /// </summary>
        /// <param name="filter">Filter to Remove</param>
        public static void RemoveFilter(AExtensionFilter filter)
        {
            if (Filters.Contains(filter))
                Filters.Remove(filter);
        }

        internal static string FilterList()
        {
            StringBuilder sb = new StringBuilder();
            foreach (AExtensionFilter aExtensionFilter in Filters)
            {
                sb.AppendLine(aExtensionFilter.ToString());
            }
            return sb.ToString();
        }
    }
}