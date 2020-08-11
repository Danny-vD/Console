using System;

namespace Console.Core.Attributes.CommandSystem
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SelectionPropertyAttribute : Attribute
    {
        public readonly bool NoConverter;
        public SelectionPropertyAttribute(bool noConverter = false)
        {
            NoConverter = noConverter;
        }


        public override string ToString()
        {
            return $"SelectionProperty{(NoConverter ? "" : "(NoConverter)")}";
        }
    }
}