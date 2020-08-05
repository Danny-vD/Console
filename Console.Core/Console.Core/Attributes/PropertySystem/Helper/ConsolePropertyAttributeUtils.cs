using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Console;

namespace Console.Core.Attributes.PropertySystem.Helper
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

        public static bool TrySetValue(string propertyPath, object value)
        {
            if (!Properties.ContainsKey(propertyPath)) return false;
            Properties[propertyPath].SetValue(value);
            return true;
        }

        public static void InitializePropertySystem()
        {
            CommandAttributeUtils.AddCommands<ConsolePropertyAttributeUtils>();
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
        private static void SetProperty(string propertyPath, [SelectionProperty] object propertyValue)
        {
            if (!HasProperty(propertyPath)) return;
            
            Properties[propertyPath].SetValue(propertyValue);
        }

        [Command("get-property",
            "Gets the value of the specified property and prints its ToString implementation to the console.", "gp")]
        private static void GetProperty(string propertyPath)
        {
            if (!HasProperty(propertyPath) || !TryGetValue(propertyPath, out object value))
            {
                AConsoleManager.Instance.LogError("Can not find property path: " + propertyPath);
                return;
            }
            AConsoleManager.Instance.Log($"{propertyPath} = {value}");
        }

        public static void AddProperties<T>()
        {
            AddRefHelpers(GetStaticConsoleFields<ConsolePropertyAttribute>(typeof(T)));
            AddRefHelpers(GetStaticConsoleProps<ConsolePropertyAttribute>(typeof(T)));
        }

        public static void AddProperties(object instance)
        {
            if (instance == null) return;
            AddRefHelpers(GetConsoleFields<ConsolePropertyAttribute>(instance));
            AddRefHelpers(GetConsoleProps<ConsolePropertyAttribute>(instance));
        }

        private static void AddRefHelpers(Dictionary<ConsolePropertyAttribute, ReflectionHelper> infos)
        {
            foreach (KeyValuePair<ConsolePropertyAttribute, ReflectionHelper> propertyInfo in infos)
            {
                Properties[propertyInfo.Key.PropertyPath] = propertyInfo.Value;
            }
        }

        #region Reflection Helper Functions

        private static Dictionary<T, ReflectionHelper> GetStaticConsoleProps<T>(Type t) where T : Attribute
        {
            return GetPropertiesWithAttribute<T>(t, BindingFlags.Static).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new StaticPropertyHelper(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }
        private static Dictionary<T, ReflectionHelper> GetStaticConsoleFields<T>(Type t) where T : Attribute
        {
            return GetFieldsWithAttribute<T>(t, BindingFlags.Static).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new StaticFieldHelper(x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }

        private static Dictionary<T, ReflectionHelper> GetConsoleProps<T>(object instance) where T : Attribute
        {
            return GetPropertiesWithAttribute<T>(instance.GetType(), BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new PropertyHelper(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }
        private static Dictionary<T, ReflectionHelper> GetConsoleFields<T>(object instance) where T : Attribute
        {
            return GetFieldsWithAttribute<T>(instance.GetType(), BindingFlags.Instance).Select(x =>
                new KeyValuePair<T, ReflectionHelper>(x.Key,
                    new FieldHelper(instance, x.Value))).ToDictionary(x => x.Key, x => x.Value);
        }


        #endregion

        #region Attribute Inspection

        private static Dictionary<T, FieldInfo> GetFieldsWithAttribute<T>(Type t, BindingFlags flag)
            where T : Attribute
        {
            return t.GetFields(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0)
                .ToDictionary(x => x.GetCustomAttribute<T>(), x => x);
        }

        private static Dictionary<T, PropertyInfo> GetPropertiesWithAttribute<T>(Type t, BindingFlags flag) where T : Attribute
        {
            return t.GetProperties(flag | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes<T>().Count() != 0)
                .ToDictionary(x => x.GetCustomAttribute<T>(), x => x);
        }

        #endregion
    }
}