using System;
using System.Collections.Generic;
using System.Linq;
using Console.Attributes.PropertySystem.Helper;

namespace Assets.Console.EnvironmentVariables
{
    public abstract class VariableProvider
    {
        public abstract string FunctionName { get; }
        public abstract string GetValue(string parameter);
    }

    public static class EnvironmentVariableManager
    {
        static EnvironmentVariableManager()
        {
            AddProvider(DefaultVariables.Instance);
        }

        public static readonly Dictionary<string, VariableProvider> Providers = new Dictionary<string, VariableProvider>();
        public static string Expand(string cmd)
        {
            string ret = cmd;
            int idx;
            while ((idx = ret.IndexOf("$", StringComparison.InvariantCulture)) != -1)
            {

                int bracketOpen = ret.IndexOf('(', idx);
                int funcLen = bracketOpen - idx - 1;
                string funcName = ret.Substring(idx + 1, funcLen);
                int bracketClose = ret.LastIndexOf(')');
                string content = ret.Substring(bracketOpen + 1, bracketClose - bracketOpen - 1);
                string expandedContent = Expand(content);
                string exp;

                string rep = $"${funcName}({content})";
                if (Providers.TryGetValue(funcName, out VariableProvider prov))
                {
                    exp = prov.GetValue(expandedContent);
                }
                else
                {
                    exp = "PARSE_ERROR";
                }

                ret = ret.Replace(rep, exp);
            }

            return ret;
        }

        public static void AddProvider(VariableProvider provider)
        {
            Providers[provider.FunctionName] = provider;
        }

        public static void RemoveProvider(string funcName)
        {
            Providers.Remove(funcName);
        }

    }
}