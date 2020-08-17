﻿using System;
using Console.Core.ReflectionSystem;

namespace Console.Core.PropertySystem
{
    public class PropertyAttributeUtils
    {
        #region Add Properties

        /// <summary>
        /// Adds Static Fields and Properties to the Property System
        /// </summary>
        /// <param name="t">Type containing the Properties</param>
        public static void AddPropertiesByType(Type t)
        {
            PropertyManager.AddRefHelpers(ReflectionUtils.GetStaticConsoleFields<PropertyAttribute>(t));
            PropertyManager.AddRefHelpers(ReflectionUtils.GetStaticConsoleProps<PropertyAttribute>(t));
        }

        /// <summary>
        /// Adds Static Fields and Properties to the Property System
        /// </summary>
        /// <typeparam name="T">Type containing the Properties</typeparam>
        public static void AddProperties<T>()
        {
            AddPropertiesByType(typeof(T));
        }

        /// <summary>
        /// Adds Instance Fields and Properties to the Property System
        /// </summary>
        /// <param name="instance">Object Instance containing the Properties</param>
        public static void AddProperties(object instance)
        {
            if (instance == null) return;
            PropertyManager.AddRefHelpers(ReflectionUtils.GetConsoleFields<PropertyAttribute>(instance));
            PropertyManager.AddRefHelpers(ReflectionUtils.GetConsoleProps<PropertyAttribute>(instance));
        }


        #endregion
    }
}