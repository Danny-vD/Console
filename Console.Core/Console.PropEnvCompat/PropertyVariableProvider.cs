using System.Collections.Generic;
using Console.Core.PropertySystem;
using Console.EnvironmentVariables;

namespace Console.PropEnvCompat
{
    /// <summary>
    /// VariableContainer Implementation with the FuncName: "props"
    /// </summary>
    public class PropertyVariableProvider : VariableContainer
    {
        /// <summary>
        /// All Environment Variables inside this container
        /// </summary>
        protected override string EnvList
        {
            get
            {
                string s = base.EnvList + "; ";
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

        /// <summary>
        /// Public Constructor
        /// </summary>
        public PropertyVariableProvider() : base("props")
        {
        }

        /// <summary>
        /// Returns the Value of the Property
        /// </summary>
        /// <param name="parameter">The Property in this container</param>
        /// <returns>The property value or NO_VALUE when the property is not found</returns>
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