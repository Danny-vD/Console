using System;
using System.Collections.Generic;
using System.Linq;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Console;
using Console.Core.Utils.Reflection;

namespace Console.Core.Utils
{
    public class ConsolePropertyAttributeUtils
    {


        private static readonly Dictionary<string, ReflectionHelper> Properties = new Dictionary<string, ReflectionHelper>();
        public static List<string> AllPropertyPaths => Properties.Keys.ToList();

        public static bool HasProperty(string propertyPath) => Properties.ContainsKey(propertyPath);

        public static bool TryGetValue(string propertyPath, out object value)
        {
            value = null;
            if (!Properties.ContainsKey(propertyPath)) return false;
            value = Properties[propertyPath].GetValue();
            return true;
        }

        public static void SetProperty(string propertyPath, ReflectionHelper helper)
        {
            Properties[propertyPath] = helper;
        }

        public static bool TrySetValue(string propertyPath, object value, bool create = false)
        {
            if (!Properties.ContainsKey(propertyPath))
            {
                if (create)
                {
                    Properties.Add(propertyPath, new FakeReflectionHelper(value));
                    return true;
                }

                return false;
            }
            Properties[propertyPath].SetValue(value);
            return true;
        }

        public static void InitializePropertySystem()
        {
            CommandAttributeUtils.AddCommands<ConsolePropertyAttributeUtils>();
            AddProperties<ConsoleCoreConfig>();
        }


        [Command("list-properties", "Lists all Properties", "lp")]
        private static void ListPropertiesCommand()
        {
            ListPropertiesCommand(string.Empty);
        }

        [Command("list-properties", "Lists all Properties that start with the specified sequence", "lp")]
        private static void ListPropertiesCommand(string start)
        {
            string ret = "Properties:\n\t";
            List<string> p = AllPropertyPaths;
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

        [Command("set-property", "Sets the specified property to the specified value", "sp")]
        private static void SetProperty(string propertyPath, [SelectionProperty(true)] object propertyValue)
        {
            if (!HasProperty(propertyPath)) return;

            AConsoleManager.Instance.Log("Setting Property: " + propertyPath + " to Value: " + propertyValue);
            Properties[propertyPath].SetValue(propertyValue);
        }

        [Command("add-property", "Sets or Adds the specified property to the specified value", "ap")]
        private static void AddProperty(string propertyPath, [SelectionProperty(true)] object propertyValue)
        {
            if (!HasProperty(propertyPath))
            {
                Properties.Add(propertyPath, new FakeReflectionHelper(propertyValue));
                return;
            }

            AConsoleManager.Instance.Log("Setting Property: " + propertyPath + " to Value: " + propertyValue);
            Properties[propertyPath].SetValue(propertyValue);
        }

        [Command("get-property",
            "Gets the value of the specified property and prints its ToString implementation to the console.", "gp")]
        private static void GetProperty(string propertyPath)
        {
            if (!HasProperty(propertyPath))
            {
                List<string> targets = AllPropertyPaths.Where(x => x.StartsWith(propertyPath)).ToList();
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
                        if (!TryGetValue(target, out v)) v = "ERROR";
                        s += $"\n\t{target} = {v}";
                    }
                    AConsoleManager.Instance.Log(s);
                }
                return;
            }
            if (!TryGetValue(propertyPath, out object value))
            {
                AConsoleManager.Instance.LogWarning("Can not get the value at path: " + propertyPath);
            }
            AConsoleManager.Instance.Log($"{propertyPath} = {value}");
        }

        public static void AddPropertiesByType(Type t)
        {
            AddRefHelpers(ReflectionUtils.GetStaticConsoleFields<ConsolePropertyAttribute>(t));
            AddRefHelpers(ReflectionUtils.GetStaticConsoleProps<ConsolePropertyAttribute>(t));
        }


        public static void AddProperties<T>()
        {
            AddPropertiesByType(typeof(T));
        }

        public static void AddProperties(object instance)
        {
            if (instance == null) return;
            AddRefHelpers( ReflectionUtils.GetConsoleFields<ConsolePropertyAttribute>(instance));
            AddRefHelpers(ReflectionUtils.GetConsoleProps<ConsolePropertyAttribute>(instance));
        }

        private static void AddRefHelpers(Dictionary<ConsolePropertyAttribute, ReflectionHelper> infos)
        {
            foreach (KeyValuePair<ConsolePropertyAttribute, ReflectionHelper> propertyInfo in infos)
            {
                Properties[propertyInfo.Key.PropertyPath] = propertyInfo.Value;
            }
        }

       
    }
}