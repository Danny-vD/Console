using System;
using System.Collections.Generic;
using Console.Core;
using Console.Core.Console;

namespace Console.EnvironmentVariables
{
    public class EnvInitializer : AExtensionInitializer
    {
        private class EnvExpander : AExpander
        {
            public override string Expand(string input)
            {
                return EnvironmentVariableManager.Expand(input);
            }
        }

        public override void Initialize()
        {
            EnvironmentVariableManager.AddProvider(DefaultVariables.Instance);
            AConsoleManager.ExpanderManager.AddExpander(new EnvExpander());
        }
    }

    public abstract class VariableProvider
    {
        public abstract string FunctionName { get; }
        public abstract string GetValue(string parameter);
    }

    public static class EnvironmentVariableManager
    {

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
                int bracketClose = FindClosing(ret);
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

        private static int FindClosing(string cmd)
        {
            int open = 0;
            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == '(') open++;
                else if (cmd[i] == ')')
                {
                    open--;
                    if (open == 0) return i;
                }
            }
            return -1;
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