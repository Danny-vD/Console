using System;
using System.Collections.Generic;
using System.Linq;

namespace Console.Core
{
    public class ParameterCollection
    {
        private static ParameterCollection Current;
        private Dictionary<string, string> Params;
        private ParameterCollection() => Params = new Dictionary<string, string>();
        public static void MakeCurrent(ParameterCollection collection) => Current = collection;
        public static string GetParameter(string name) => Current.GetParameterValue(name);

        private string GetParameterValue(string name)
        {
            if (Params.ContainsKey(name)) return Params[name];
            return name;
        }

        public static ParameterCollection CreateCollection(string[] sig, string parameter)
        {
            ParameterCollection c = new ParameterCollection();

            string[] p = parameter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            int max = Math.Min(sig.Length, p.Length);
            for (int i = 0; i < max; i++)
            {
                c.Params[sig[i]] = p[i];
            }
            return c;
        }
    }
}