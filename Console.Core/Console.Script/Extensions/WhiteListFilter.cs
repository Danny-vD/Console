namespace Console.Script.Extensions
{
    /// <summary>
    /// All Extensions in this filter are explicitly allowed.
    /// </summary>
    public class WhiteListFilter : ListFilter
    {

        private static WhiteListFilter Filter;


        /// <summary>
        /// Public Constructor
        /// </summary>
        public WhiteListFilter() : base(new string[0])
        {
            if (Filter == null)
            {
                Filter = this;
            }
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="file">File(extension seperated by new lines)</param>
        public WhiteListFilter(string file) : base(file)
        {
            if (Filter == null)
            {
                Filter = this;
            }
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="list">Extension List</param>
        public WhiteListFilter(string[] list) : base(list)
        {
            if (Filter == null)
            {
                Filter = this;
            }
        }

        /// <summary>
        /// Returns true if the extension is allowed to be loaded.
        /// </summary>
        /// <param name="extensions">Extension to Check</param>
        /// <returns>True if Allowed</returns>
        public override bool Allowed(string extensions)
        {
            return Extensions.Contains(extensions);
        }

        /// <summary>
        /// Adds an Extension to the White List
        /// </summary>
        /// <param name="extension">Extension that should be WhiteListed</param>
        public static void Add(string extension)
        {
            if (Filter != null && !Filter.Extensions.Contains(extension))
            {
                Filter.Extensions.Add(extension);
            }
        }

        /// <summary>
        /// Removes an Extension from the White List
        /// </summary>
        /// <param name="extension">Extension that should be Removed</param>
        public static void Remove(string extension)
        {
            if (Filter != null && Filter.Extensions.Contains(extension))
            {
                Filter.Extensions.Remove(extension);
            }
        }

        /// <summary>
        /// Returns all Contained Extensions
        /// </summary>
        /// <returns>Extension List</returns>
        public override string ToString()
        {
            return "Allowed Extensions: \n" + base.ToString();
        }

    }
}