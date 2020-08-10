using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;

namespace Console.EnvironmentVariables
{
    public static class EnvironmentVariableManager
    {
        public static readonly Dictionary<string, VariableProvider> Providers = new Dictionary<string, VariableProvider>();
        internal static string EnvList
        {
            get
            {
                string s = "";
                List<string> keys = Providers.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    string instanceProvider = keys[i];
                    s += instanceProvider;
                    if (i != keys.Count - 1) s += "; ";
                }
                return s;
            }
        }

        [Command("add-env-api", "Adds all public static methods with valid return and one string parameter.")]
        public static void AddStringTransformMethods(string funcPrefix, string qualifiedType)
        {
            Type t = Type.GetType(qualifiedType);

            if (t == null)
            {
                AConsoleManager.Instance.LogWarning("Can not find Type with name: " + qualifiedType);
                return;
            }
            AddStringTransformMethods(funcPrefix, t);
        }

        public static void AddStringTransformMethods(string funcPrefix, Type type)
        {
            MethodInfo[] mi = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (MethodInfo methodInfo in mi)
            {
                if (ValidType(methodInfo.ReturnType))
                {
                    if ((methodInfo.GetParameters().Length == 1 &&
                              methodInfo.GetParameters()[0].ParameterType == typeof(string)))
                    {
                        AddProvider(GetProvider(funcPrefix, methodInfo));
                    }
                }
            }
        }

        private static bool ValidType(Type t) => t.IsPrimitive || t == typeof(string);

        public static VariableProvider GetProvider(string funcPrefix, MethodInfo info)
        {
            return new DelegateVariableProvider(funcPrefix + info.Name, s => info.Invoke(null, new[] { s })?.ToString());
        }
        public static string Expand(string cmd)
        {
            string ret = cmd;
            int idx;
            while ((idx = ret.IndexOf("$", StringComparison.InvariantCulture)) != -1)
            {

                int bracketOpen = ret.IndexOf('(', idx);
                if (bracketOpen == -1) return cmd;
                int funcLen = bracketOpen - idx - 1;
                if (funcLen < 0) return cmd;
                string funcName = ret.Substring(idx + 1, funcLen);
                int bracketClose = FindClosing(ret);
                if (bracketClose == -1) return cmd;
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