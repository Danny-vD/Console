
using System.Collections.Generic;
using System.Linq;
using Console.Core;
using Console.Core.Utils;
using Console.EnvironmentVariables;

namespace Console.PropEnvCompat
{
    public class PropCompatInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            EnvironmentVariableManager.AddProvider(new PropertyVariableProvider());
        }
    }

    public class PropertyVariableProvider : VariableContainer
    {
        protected override string EnvList
        {
            get
            {
                string s = base.EnvList+"; ";
                List<string> keys = ConsolePropertyAttributeUtils.AllPropertyPaths;
                for (int i = 0; i < keys.Count; i++)
                {
                    string instanceProvider = keys[i];
                    s += instanceProvider;
                    if (i != keys.Count - 1) s += "; ";
                }
                return s;
            }
        }

        public PropertyVariableProvider() : base("props") { }

        public override string GetValue(string parameter)
        {
            if (Providers.ContainsKey(parameter))
            {
                return Providers[parameter].GetValue(parameter);
            }
            if (ConsolePropertyAttributeUtils.TryGetValue(parameter, out object ret))
            {
                return ret.ToString();
            }
            return "NO_VALUE";
        }
    }
}