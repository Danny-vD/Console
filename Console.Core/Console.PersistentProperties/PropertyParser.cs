using System;
using System.Collections.Generic;
using System.Linq;

namespace Console.PersistentProperties
{
    internal static class PropertyParser
    {
        public static Dictionary<string, string> LoadProperties(string content)
        {
            List<string> lines = content.Split(new[] {'\n'}).Select(x => x.Trim())
                .Select(x => x.Split(new[] {'#'}, StringSplitOptions.None).First()).Where(x=>!string.IsNullOrEmpty(x))
                .ToList();

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                string[] vs = line.Split(new[] { '=' }, StringSplitOptions.None);
                values[vs[0].Trim()] = vs[1].Trim();
            }
            return values;
        }
    }
}