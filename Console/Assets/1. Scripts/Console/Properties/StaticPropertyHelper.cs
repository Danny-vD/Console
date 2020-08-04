using System.Reflection;

namespace Console.Properties
{
    internal class StaticPropertyHelper : PropertyHelper
    {
        internal StaticPropertyHelper(PropertyInfo info) : base(null, info) { }
    }
}