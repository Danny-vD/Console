using System;

namespace Console.Core.PropertySystem
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyAttribute : Attribute
    {
        public readonly string PropertyPath;

        public PropertyAttribute(string propertyPath)
        {
            PropertyPath = propertyPath;
        }
    }
}