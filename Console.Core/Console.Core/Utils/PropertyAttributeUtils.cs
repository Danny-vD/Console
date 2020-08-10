using System;
using Console.Core.PropertySystem;

namespace Console.Core.Utils
{
    public class PropertyAttributeUtils
    {
        #region Add Properties

        public static void AddPropertiesByType(Type t)
        {
            PropertyManager.AddRefHelpers(ReflectionUtils.GetStaticConsoleFields<PropertyAttribute>(t));
            PropertyManager.AddRefHelpers(ReflectionUtils.GetStaticConsoleProps<PropertyAttribute>(t));
        }


        public static void AddProperties<T>()
        {
            AddPropertiesByType(typeof(T));
        }

        public static void AddProperties(object instance)
        {
            if (instance == null) return;
            PropertyManager.AddRefHelpers(ReflectionUtils.GetConsoleFields<PropertyAttribute>(instance));
            PropertyManager.AddRefHelpers(ReflectionUtils.GetConsoleProps<PropertyAttribute>(instance));
        }


        #endregion
    }
}