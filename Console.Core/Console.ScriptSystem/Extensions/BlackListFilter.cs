namespace Console.ScriptSystem.Extensions
{
    /// <summary>
    /// All Extensions in this filter are explicitly not allowed.
    /// </summary>
    public class BlackListFilter : ListFilter
    {
        private static BlackListFilter Filter;

        /// <summary>
        /// Public Constructor
        /// </summary>
        public BlackListFilter() : base(new string[0])
        {
            if (Filter == null)
                Filter = this;
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="file">File(extension seperated by new lines)</param>
        public BlackListFilter(string file) : base(file)
        {
            if (Filter == null)
                Filter = this;
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="list">Extension List</param>
        public BlackListFilter(string[] list) : base(list)
        {
            if (Filter == null)
                Filter = this;
        }

        /// <summary>
        /// Returns true if the extension is allowed to be loaded.
        /// </summary>
        /// <param name="extensions">Extension to Check</param>
        /// <returns>True if Allowed</returns>
        public override bool Allowed(string extensions)
        {
            return !Extensions.Contains(extensions);
        }

        /// <summary>
        /// Adds an Extension to the Black List
        /// </summary>
        /// <param name="extension">Extension that should be BlackListed</param>
        public static void Add(string extension)
        {
            if (Filter != null && !Filter.Extensions.Contains(extension))
                Filter.Extensions.Add(extension);
        }

        /// <summary>
        /// Removes an Extension from the Black List
        /// </summary>
        /// <param name="extension">Extension that should be Removed</param>
        public static void Remove(string extension)
        {
            if (Filter != null && Filter.Extensions.Contains(extension))
                Filter.Extensions.Remove(extension);
        }

        /// <summary>
        /// Returns all Contained Extensions
        /// </summary>
        /// <returns>Extension List</returns>
        public override string ToString()
        {
            return "Disallowed Extensions: \n" + base.ToString();
        }
    }
}