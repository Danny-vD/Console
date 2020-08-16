using System.Collections.Generic;
using System.Linq;
using Console.Core.PropertySystem;

namespace Console.Core.CommandSystem.Commands.BuiltIn
{
    public class PropertyCommands
    {

        public static void AddPropertyCommands()
        {
            CommandAttributeUtils.AddCommands<PropertyCommands>();
        }

        #region List Properties

        [Command("list-properties", "Lists all Properties", "lp")]
        private static void ListPropertiesCommand()
        {
            ListPropertiesCommand(string.Empty);
        }

        [Command("list-properties", "Lists all Properties that start with the specified sequence", "lp")]
        private static void ListPropertiesCommand(string start)
        {
            string ret = "Properties:\n\t";
            List<string> p = PropertyManager.AllPropertyPaths;
            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].StartsWith(start))
                {
                    ret += p[i];
                    if (i != p.Count - 1) ret += "\n\t";
                }
            }
            AConsoleManager.Instance.Log(ret);
        }

        #endregion

        #region Get Properties

        [Command("get-property",
            "Gets the value of the specified property and prints its ToString implementation to the console.", "gp")]
        private static void GetProperty() => GetProperty("");

        [Command("get-property",
            "Gets the value of the specified property and prints its ToString implementation to the console.", "gp")]
        private static void GetProperty(string propertyPath)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                List<string> targets = PropertyManager.AllPropertyPaths.Where(x => x.StartsWith(propertyPath)).ToList();
                if (targets.Count == 0)
                {
                    AConsoleManager.Instance.LogWarning("Can not find property path: " + propertyPath);
                }
                else
                {
                    string s = "Properties that match: " + propertyPath;
                    foreach (string target in targets)
                    {
                        object v;
                        if (!PropertyManager.TryGetValue(target, out v)) v = "ERROR";
                        s += $"\n\t{target} = {v ?? "NULL"}";
                    }
                    AConsoleManager.Instance.Log(s);
                }
                return;
            }
            if (!PropertyManager.TryGetValue(propertyPath, out object value))
            {
                AConsoleManager.Instance.LogWarning("Can not get the value at path: " + propertyPath);
            }
            AConsoleManager.Instance.Log($"{propertyPath} = {value}");
        }

        #endregion

        #region Set/Add Properties

        [Command("set-property-selection", "Sets the specified property to the specified value", "sps")]
        private static void SetPropertySelection(string propertyPath, [SelectionProperty(true)] object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath)) return;

            AConsoleManager.Instance.Log("Setting Property: " + propertyPath + " to Value: " + propertyValue);
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        [Command("add-property-selection", "Sets or Adds the specified property to the specified value", "aps")]
        private static void AddPropertySelection(string propertyPath, [SelectionProperty(true)] object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                PropertyManager.AddProperty(propertyPath, propertyValue);
                return;
            }

            AConsoleManager.Instance.Log("Setting Property: " + propertyPath + " to Value: " + propertyValue);
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }
        [Command("set-property", "Sets the specified property to the specified value", "sp")]
        private static void SetProperty(string propertyPath, object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath)) return;

            AConsoleManager.Instance.Log("Setting Property: " + propertyPath + " to Value: " + propertyValue);
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        [Command("add-property", "Sets or Adds the specified property to the specified value", "ap")]
        private static void AddProperty(string propertyPath, object propertyValue)
        {
            if (!PropertyManager.HasProperty(propertyPath))
            {
                PropertyManager.AddProperty(propertyPath, propertyValue);
                return;
            }

            AConsoleManager.Instance.Log("Setting Property: " + propertyPath + " to Value: " + propertyValue);
            PropertyManager.SetPropertyValue(propertyPath, propertyValue);
        }

        #endregion
    }
}