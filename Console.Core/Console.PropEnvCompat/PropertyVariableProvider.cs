using System.Collections.Generic;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;

namespace Console.PropEnvCompat
{
    public class PropertyVariableProvider : VariableContainer
    {
        protected override string EnvList
        {
            get
            {
                string s = base.EnvList+"; ";
                List<string> keys = PropertyManager.AllPropertyPaths;
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
            if (PropertyManager.TryGetValue(parameter, out object ret))
            {
                return ret.ToString();
            }
            return "NO_VALUE";
        }
    }
}