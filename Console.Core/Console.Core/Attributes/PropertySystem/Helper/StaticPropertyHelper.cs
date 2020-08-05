using System.Reflection;

namespace Console.Core.Attributes.PropertySystem.Helper
{
    internal class StaticPropertyHelper : PropertyHelper
    {
        internal StaticPropertyHelper(PropertyInfo info) : base(null, info) { }
    }
}