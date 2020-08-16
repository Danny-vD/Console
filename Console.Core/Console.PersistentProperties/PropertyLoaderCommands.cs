﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Console.Core.CommandSystem;
using Console.Core.PropertySystem;

namespace Console.PersistentProperties
{
    public class PropertyLoaderCommands
    {
        [Command("load-properties", "Load a settings file")]
        public static void Load(string file)
        {
            if (File.Exists(file))
            {
                Dictionary<string, string> v = PropertyParser.LoadProperties(File.ReadAllText(file));
                v.ToList().ForEach(x => PropertyManager.TrySetValue(x.Key, x.Value));
            }
        }


        [Command("save-properties", "Save all properties to file")]
        public static void Save(string file) => Save(file, "");

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