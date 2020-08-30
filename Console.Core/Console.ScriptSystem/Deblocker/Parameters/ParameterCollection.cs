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

        private ParameterCollection()
        {
            Params = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sets the collection as the Current Collection that gets used by the $param() expander
        /// </summary>
        /// <param name="collection"></param>
        public static void MakeCurrent(ParameterCollection collection)
        {
            Current = collection;
        }


        /// <summary>
        /// Returns the Value of the Parameter
        /// If the Value is not found the ParameterName is returned
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <returns>The Value of the Parameter</returns>
        public static string GetParameter(string name)
        {
            return Current != null ? Current.GetParameterValue(name) : name;
        }

        private string GetParameterValue(string name)
        {
            if (Params.ContainsKey(name))
            {
                return Params[name];
            }
            return name;
        }

        private ParameterCollection MakeSub(ParameterCollection child)
        {
            ParameterCollection ret = new ParameterCollection {Params = new Dictionary<string, string>(Params)};
            foreach (KeyValuePair<string, string> keyValuePair in child.Params)
            {
                ret.Params[keyValuePair.Key] = keyValuePair.Value;
            }
            return ret;
        }

        /// <summary>
        /// Creates a Collection based on the Current Collection
        /// </summary>
        /// <param name="sig">Parameter Signature</param>
        /// <param name="parameter">Parameter Values</param>
        /// <returns></returns>
        public static ParameterCollection CreateSubCollection(string[] sig, string parameter)
        {
            ParameterCollection child = CreateCollection(sig, parameter);
            if (Current == null)
            {
                return child;
            }
            return Current.MakeSub(child);
        }

        /// <summary>
        /// Creates a Collection
        /// </summary>
        /// <param name="sig">Parameter Signature</param>
        /// <param name="parameter">Parameter Values</param>
        /// <returns></returns>
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