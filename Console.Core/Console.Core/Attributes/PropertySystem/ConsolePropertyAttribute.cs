using System;

namespace Console.Core.Attributes.PropertySystem
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = true)]
    public class ConsolePropertyAttribute : Attribute
    {
        public readonly string PropertyPath;

        public ConsolePropertyAttribute(string propertyPath)
        {
            PropertyPath = propertyPath;
        }
    }
}