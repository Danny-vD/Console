using System;

namespace Console.Core.PropertySystem
{
    /// <summary>
    /// Property Attribute.
    /// When a Field or Property is Decorated with this attribute it will be loaded by the Property System.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyAttribute : ConsoleAttribute
    {

        /// <summary>
        /// The Property Path/Name
        /// </summary>
        public readonly string PropertyPath;

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="propertyPath">Path/Name of the Property</param>
        public PropertyAttribute(string propertyPath)
        {
            PropertyPath = propertyPath;
        }

    }
}