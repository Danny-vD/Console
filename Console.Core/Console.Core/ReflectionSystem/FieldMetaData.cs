using System;
using System.Reflection;

using Console.Core.CommandSystem.Attributes;
using Console.Core.ReflectionSystem.Abstract;
using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// IValueTypeContainer Implementation using a FieldInfo type as backend
    /// </summary>
    public class FieldMetaData : AInstancedMetaData<FieldInfo>, IValueTypeContainer
    {

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="instance">Instance bound to the Field Info</param>
        /// <param name="info">Field Info used as Backend</param>
        public FieldMetaData(object instance, FieldInfo info) : base(instance, info)
        {
        }

        /// <summary>
        /// Flag that determines if the Value can be Written
        /// </summary>
        public bool CanWrite => !ReflectedInfo.IsInitOnly;

        /// <summary>
        /// Flag that determines if the Value can be Read
        /// </summary>
        public bool CanRead => true;

        /// <summary>
        /// The Type of the Value
        /// </summary>
        public Type ValueType => ReflectedInfo.FieldType;

        /// <summary>
        /// Sets the Value of the Field Info to the specified value
        /// </summary>
        /// <param name="value">New Value</param>
        public void Set(object value)
        {
            ReflectedInfo.Set(Instance, CommandAttributeUtils.ConvertToNonGeneric(value, ValueType));
        }

        /// <summary>
        /// Gets the Value of the Field Info
        /// </summary>
        /// <returns>Value of the Property</returns>
        public object Get()
        {
            return ReflectedInfo.Get(Instance);
        }

        /// <summary>
        /// Gets the Value of the Field Info
        /// </summary>
        /// <typeparam name="T">Type of the Property</typeparam>
        /// <returns>Value of the Property</returns>
        public T Get<T>()
        {
            return (T) Get();
        }

    }
}