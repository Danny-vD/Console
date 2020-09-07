using System.Collections.Generic;
using System.Linq;

namespace Console.Vars
{
    /// <summary>
    /// Variable Container Implementation.
    /// Allows for Grouping Environment Variables
    /// </summary>
    public class VariableContainer : VariableProvider
    {

        /// <summary>
        /// Contained Providers
        /// </summary>
        protected readonly Dictionary<string, VariableProvider> Providers = new Dictionary<string, VariableProvider>();

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="funcName">Name of the Variable Container.</param>
        public VariableContainer(string funcName)
        {
            FunctionName = funcName;
            AddProvider(new DelegateVariableProvider("envs", s => EnvList));
        }

        /// <summary>
        /// ; Seperated list of contained VariableProviders.
        /// </summary>
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
                    if (i != keys.Count - 1)
                    {
                        s += "; ";
                    }
                }

                return s;
            }
        }

        /// <summary>
        /// Name of the Container Function
        /// </summary>
        public override string FunctionName { get; }

        /// <summary>
        /// Returns the Value of the Provider
        /// </summary>
        /// <param name="parameter">Input Data</param>
        /// <returns>Value of the Provider</returns>
        public override string GetValue(string parameter)
        {
            if (Providers.TryGetValue(parameter, out VariableProvider prov))
            {
                return prov.GetValue(parameter);
            }

            return "NO_VALUE";
        }

        /// <summary>
        /// Adds a Provider to the EnvironmentVariable Manager.
        /// </summary>
        /// <param name="provider">Provider to Add</param>
        public void AddProvider(VariableProvider provider)
        {
            Providers[provider.FunctionName] = provider;
        }


        /// <summary>
        /// Removes a Provider from the EnvironmentVariable Manager.
        /// </summary>
        /// <param name="providerName">Name of the Provider to Remove</param>
        public void RemoveProvider(string providerName)
        {
            Providers.Remove(providerName);
        }

    }
}