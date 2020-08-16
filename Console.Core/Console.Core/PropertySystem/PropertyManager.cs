using System.Collections.Generic;
using System.Linq;
using Console.Core.ReflectionSystem;
using Console.Core.ReflectionSystem.Interfaces;
namespace Console.Core.PropertySystem
{
    public static class PropertyManager
    {
        private static readonly Dictionary<string, IValueTypeContainer> Properties = new Dictionary<string, IValueTypeContainer>();
        public static List<string> AllPropertyPaths
        {
            get
            {
                List<string> r = Properties.Keys.ToList();
                r.Sort();
                return r;
            }
        }

        public static bool HasProperty(string propertyPath) => Properties.ContainsKey(propertyPath);
        public static void AddProperty(string propertyPath, object value)
        {
            if (!HasProperty(propertyPath))
                SetProperty(propertyPath, new FakeValueContainer(value));
            else
                SetPropertyValue(propertyPath, value);
        }

        public static void SetProperty(string propertyPath, IValueTypeContainer helper)
        {
            Properties[propertyPath] = helper;
        }
        // public static void SetProperty(string propertyPath, object value) => SetProperty(propertyPath, new FakeReflectionHelper(value));


        #region Try Get/Set

        public static object GetPropertyValue(string propertyPath) => Properties[propertyPath].Get();
        public static bool TryGetValue(string propertyPath, out object value)
        {
            value = null;
            if (!HasProperty(propertyPath)) return false;
            value = GetPropertyValue(propertyPath);
            return true;
        }

        public static void SetPropertyValue(string propertyPath, object value)
        {
            if (Properties[propertyPath].CanWrite)
                Properties[propertyPath].Set(value);
            else
                AConsoleManager.Instance.LogWarning("Can not Write property: " + propertyPath + " its already existing and readonly");
        }
        public static bool TrySetValue(string propertyPath, object value, bool create = false)
        {
            if (!HasProperty(propertyPath))
            {
                if (create)
                {
                    AddProperty(propertyPath, value);
                    return true;
                }
                return false;
            }
            SetPropertyValue(propertyPath, value);
            return true;
        }

        #endregion


        internal static void AddRefHelpers(Dictionary<PropertyAttribute, IValueTypeContainer> infos)
        {
            foreach (KeyValuePair<PropertyAttribute, IValueTypeContainer> propertyInfo in infos)
            {
                Properties[propertyInfo.Key.PropertyPath] = propertyInfo.Value;
            }
        }
    }
}