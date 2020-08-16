using System.Reflection;

namespace Console.Core.ReflectionSystem
{
    public class StaticPropertyMetaData : PropertyMetaData
    {
        public StaticPropertyMetaData(PropertyInfo info) : base(null, info) { }
    }
}