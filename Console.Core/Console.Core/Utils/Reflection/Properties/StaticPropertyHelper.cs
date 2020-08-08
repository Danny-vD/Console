using System.Reflection;

namespace Console.Core.Utils.Reflection.Properties
{
    public class StaticPropertyHelper : PropertyHelper
    {
        public StaticPropertyHelper(PropertyInfo info) : base(null, info) { }
    }
}