using System;
using System.Collections.Generic;
using System.Linq;

namespace Console.ScriptSystem.Deblocker.Parameters
{
    /// <summary>
    /// A ParameterCollection is a Collection of parameter names from blocks
    /// </summary>
    public class ParameterCollection
    {
        private static ParameterCollection Current;
        private Dictionary<string, string> Params;
        private ParameterCollection() => Params = new Dictionary<string, string>();
        public static void MakeCurrent(ParameterCollection collection) => Current = collection;


        public static string GetParameter(string name) => Current != null ? Current.GetParameterValue(name) : name;

        private string GetParameterValue(string name)
        {
            if (Params.ContainsKey(name)) return Params[name];
            return name;
        }

        private ParameterCollection MakeSub(ParameterCollection child)
        {
            ParameterCollection ret = new ParameterCollection();
            ret.Params = new Dictionary<string, string>(Params);
            foreach (KeyValuePair<string, string> keyValuePair in child.Params)
            {
                ret.Params[keyValuePair.Key] = keyValuePair.Value;
            }
            return ret;
        }

        public static ParameterCollection CreateSubCollection(string[] sig, string parameter)
        {
            ParameterCollection child = CreateCollection(sig, parameter);
            if (Current == null)
            {
                return child;
            }
            return Current.MakeSub(child);
        }

        public static ParameterCollection CreateCollection(string[] sig, string parameter)
        {
            ParameterCollection c = new ParameterCollection();

            string[] p = parameter.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
                .ToArray();
            int max = Math.Min(sig.Length, p.Length);
            for (int i = 0; i < max; i++)
            {
                c.Params[sig[i]] = p[i];
            }
            return c;
        }
    }
}