using System.Reflection;

namespace Console.Attributes.PropertySystem.Helper
{
    internal class StaticPropertyHelper : PropertyHelper
    {
        internal StaticPropertyHelper(PropertyInfo info) : base(null, info) { }
    }
}