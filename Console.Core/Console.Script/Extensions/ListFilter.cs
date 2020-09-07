using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console.Script.Extensions
{
    /// <summary>
    /// Abstract List Filter. Does Implement an Extension List
    /// </summary>
    public abstract class ListFilter : AExtensionFilter
    {

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="extensions">Extension List</param>
        protected ListFilter(string[] extensions)
        {
            Extensions = extensions.ToList();
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="file">File(extension seperated by new lines)</param>
        protected ListFilter(string file)
        {
            if (File.Exists(file))
            {
                Extensions = File.ReadAllLines(file).ToList();
            }
            else
            {
                Extensions = new List<string>();
            }
        }

        /// <summary>
        /// The Extension List used to Allow/Disallow Extensions
        /// </summary>
        protected List<string> Extensions { get; }


        /// <summary>
        /// Returns all Contained Extensions
        /// </summary>
        /// <returns>Extension List</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string extension in Extensions)
            {
                sb.AppendLine("\t" + extension);
            }

            return sb.ToString();
        }

    }
}