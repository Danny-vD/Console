using System.Reflection;

namespace Console.Core.Attributes.PropertySystem.Helper
{
    public class StaticPropertyHelper : PropertyHelper
    {
        public StaticPropertyHelper(PropertyInfo info) : base(null, info) { }
    }
}