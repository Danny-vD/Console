using System.Collections.Generic;
using System.IO;
using System.Linq;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;

namespace Console.PersistentProperties
{
    /// <summary>
    /// Property Loader Command Collection
    /// </summary>
    public class PropertyLoaderCommands
    {
        /// <summary>
        /// Loads a Settings File.
        /// </summary>
        /// <param name="file">Filepath</param>
        [Command("load-properties", "Load a settings file")]
        public static void Load(string file)
        {
            if (File.Exists(file))
            {
                Dictionary<string, string> v = PropertyParser.LoadProperties(File.ReadAllText(file));
                v.ToList().ForEach(x => PropertyManager.TrySetValue(x.Key, x.Value));
            }
        }


        /// <summary>
        /// Saves all Properties to a file
        /// </summary>
        /// <param name="file">Filepath</param>
        [Command("save-properties", "Save all properties to file")]
        public static void Save(string file) => Save(file, "");

        /// <summary>
        /// Saves all Properties that start with the search term to a file
        /// </summary>
        /// <param name="file">Filepath</param>
        /// <param name="searchTerm">Search String</param>
        [Command("save-properties", "Save all properties to file that match a search term")]
        public static void Save(string file, string searchTerm)
        {
            List<string> ret = new List<string>();
            List<string> p = PropertyManager.AllPropertyPaths.Where(x => x.StartsWith(searchTerm)).ToList();
            for (int i = 0; i < p.Count; i++)
            {
                if (PropertyManager.TryGetValue(p[i], out object obj))
                {
                    ret.Add($"{p[i]}={obj}");
                }
            }
            File.WriteAllLines(file, ret);
        }
    }
}