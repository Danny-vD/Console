using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Console.EnvironmentVariables
{

    public class DefaultVariables : VariableProvider
    {
        public static readonly DefaultVariables Instance;
        private static string EnvList
        {
            get
            {
                string s = "";
                List<string> keys = Instance.Providers.Keys.ToList();
                for (int i = 0; i < keys.Count; i++)
                {
                    string instanceProvider = keys[i];
                    s += instanceProvider;
                    if (i != keys.Count - 1) s += "; ";
                }
                return s;
            }
        }

        static DefaultVariables()
        {
            Instance = new DefaultVariables();
            Instance.AddProvider(new DelegateVariableProvider("user", s => Environment.UserName));
            Instance.AddProvider(new DelegateVariableProvider("machine", s => Environment.MachineName));
            AddProperties(typeof(Application));
            Instance.AddProvider(new DelegateVariableProvider("envs", s => EnvList));
        }

        private static void AddProperties(Type t)
        {
            PropertyInfo[] infos = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo propertyInfo in infos)
            {
                if (propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType == typeof(string))
                {
                    Instance.AddProvider(new DelegateVariableProvider(propertyInfo.Name, s => propertyInfo.GetValue(null).ToString()));
                }
            }
        }

        private DefaultVariables() { }

        private readonly Dictionary<string, VariableProvider> Providers = new Dictionary<string, VariableProvider>();

        public override string FunctionName => "";
        public override string GetValue(string parameter)
        {
            if (Providers.TryGetValue(parameter, out VariableProvider prov))
            {
                return prov.GetValue(parameter);
            }
            return "NO_VALUE";
        }

        public void AddProvider(VariableProvider provider)
        {
            Providers[provider.FunctionName] = provider;
        }

        public void RemoveProvider(string providerName) => Providers.Remove(providerName);

    }

    public class DelegateVariableProvider : VariableProvider
    {
        private readonly Func<string, string> Provider;
        public override string FunctionName { get; }

        public override string GetValue(string parameter) => Provider?.Invoke(parameter);

        public DelegateVariableProvider(string functionName, Func<string, string> provider)
        {
            FunctionName = functionName;
            Provider = provider;
        }
    }
}