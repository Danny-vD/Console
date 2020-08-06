using System.Collections.Generic;
using System.Linq;

namespace Console.EnvironmentVariables
{
    public class VariableContainer : VariableProvider
    {

        protected readonly Dictionary<string, VariableProvider> Providers = new Dictionary<string, VariableProvider>();
        protected virtual string EnvList
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

        public override string FunctionName { get; }
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

        public VariableContainer(string funcName)
        {
            FunctionName = funcName;
            AddProvider(new DelegateVariableProvider("envs", s => EnvList));
        }
    }
}