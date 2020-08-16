using System.Collections.Generic;
using System.Linq;
using Console.Core.ReflectionSystem;
using Console.Core.ReflectionSystem.Interfaces;
namespace Console.Core.PropertySystem
{
    /// <summary>
    /// Property System API
    /// </summary>
    public static class PropertyManager
    {
        /// <summary>
        /// All Properties
        /// </summary>
        private static readonly Dictionary<string, IValueTypeContainer> Properties = new Dictionary<string, IValueTypeContainer>();
        /// <summary>
        /// List of all Property Paths/Names
        /// </summary>
        public static List<string> AllPropertyPaths
        {
            get
            {
                List<string> r = Properties.Keys.ToList();
                r.Sort();
                return r;
            }
        }

        /// <summary>
        /// Returns true if the Property Path does exist in the Loaded Properties
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <returns>True if the Property Exists</returns>
        public static bool HasProperty(string propertyPath) => Properties.ContainsKey(propertyPath);
        /// <summary>
        /// Adds or Sets the property with the specified path to the specified value
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <param name="value">New Value</param>
        public static void AddProperty(string propertyPath, object value)
        {
            if (!HasProperty(propertyPath))
                SetProperty(propertyPath, new FakeValueContainer(value));
            else
                SetPropertyValue(propertyPath, value);
        }

        /// <summary>
        /// Sets or Adds the Type container of the specified property(use with caution)
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <param name="helper">New Value Container</param>
        public static void SetProperty(string propertyPath, IValueTypeContainer helper)
        {
            Properties[propertyPath] = helper;
        }

        #region Try Get/Set

        /// <summary>
        /// Returns the Property Value at the specified name
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <returns>The Value</returns>
        public static object GetPropertyValue(string propertyPath) => Properties[propertyPath].Get();

        /// <summary>
        /// Returns false if the value could not be retrieved.
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <param name="value">New Value</param>
        /// <returns>True if the value has been retrieved.</returns>
        public static bool TryGetValue(string propertyPath, out object value)
        {
            value = null;
            if (!HasProperty(propertyPath)) return false;
            value = GetPropertyValue(propertyPath);
            return true;
        }
        /// <summary>
        /// Sets the Property Value of the Specified Path to the passed object.
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <param name="value">New Value</param>
        public static void SetPropertyValue(string propertyPath, object value)
        {
            if (Properties[propertyPath].CanWrite)
                Properties[propertyPath].Set(value);
            else
                AConsoleManager.Instance.LogWarning("Can not Write property: " + propertyPath + " its already existing and readonly");
        }

        /// <summary>
        /// Returns true if the Property Value has been set.
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        /// <param name="value">New Value</param>
        /// <param name="create">Optionally Create the Property when its not found.</param>
        /// <returns>True if the Value was set</returns>
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


        /// <summary>
        /// Adds all Properties into the System
        /// </summary>
        /// <param name="infos">Value Containers to be Added.</param>
        internal static void AddRefHelpers(Dictionary<PropertyAttribute, IValueTypeContainer> infos)
        {
            foreach (KeyValuePair<PropertyAttribute, IValueTypeContainer> propertyInfo in infos)
            {
                Properties[propertyInfo.Key.PropertyPath] = propertyInfo.Value;
            }
        }
    }
}