using System;
using System.Collections.Generic;

using Console.Core.ReflectionSystem.Interfaces;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// Value Type Container that does not use reflection
    /// </summary>
    public class FakeValueContainer : IValueTypeContainer
    {

        /// <summary>
        /// The Inner Value
        /// </summary>
        public object Value;

        /// <summary>
        /// Creates a Fake Value Container
        /// </summary>
        /// <param name="value">Value</param>
        public FakeValueContainer(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Flag that determines if the Value can be Read
        /// </summary>
        public bool CanRead => true;

        /// <summary>
        /// Flag that determines if the Value can be Written
        /// </summary>
        public bool CanWrite => true;

        /// <summary>
        /// The Type of the Value
        /// </summary>
        public Type ValueType => Value.GetType();

        /// <summary>
        /// Collection of Attributes
        /// </summary>
        public List<Attribute> Attributes { get; }

        /// <summary>
        /// Gets the Inner Value
        /// </summary>
        /// <returns>The Inner Value</returns>
        public object Get()
        {
            return Value;
        }

        /// <summary>
        /// Sets the Inner Value
        /// </summary>
        /// <param name="value">New Value</param>
        public void Set(object value)
        {
            Value = value;
        }

    }
}