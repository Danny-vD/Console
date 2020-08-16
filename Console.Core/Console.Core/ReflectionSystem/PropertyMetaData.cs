﻿using System;
using System.Reflection;
using Console.Core.ReflectionSystem.Abstract;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// IValueTypeContainer Implementation. Using a Property Info Instance as Backend.
    /// </summary>
    public class PropertyMetaData : AInstancedMetaData<PropertyInfo>, IValueTypeContainer
    {
        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="info"></param>
        public PropertyMetaData(object instance, PropertyInfo info) : base(instance, info) { }

        /// <summary>
        /// The Type of the Property.
        /// </summary>
        public Type ValueType => ReflectedInfo.PropertyType;
        /// <summary>
        /// Flag that determines if the Value can be Written
        /// </summary>
        public bool CanWrite => ReflectedInfo.CanWrite;

        /// <summary>
        /// Flag that determines if the Value can be Read
        /// </summary>
        public bool CanRead => ReflectedInfo.CanRead;

        /// <summary>
        /// Sets the Property to the specified value
        /// </summary>
        /// <param name="value">New Value</param>
        public void Set(object value)
        {
            if (ValueType.IsInstanceOfType(value) || value == null)
            {
                ReflectedInfo.SetValue(Instance, value);
            }
        }

        /// <summary>
        /// Gets the Value of the Property
        /// </summary>
        /// <returns>Value of the Property</returns>
        public object Get() => ReflectedInfo.GetValue(Instance);

        /// <summary>
        /// Gets the Value of the Property
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <returns>Value of the Property</returns>
        public T Get<T>()
        {
            return (T)Get();
        }
    }
}