using System.Reflection;

namespace Console.Core.ReflectionSystem
{
    public class StaticFieldMetaData : FieldMetaData
    {
        public StaticFieldMetaData(FieldInfo info) : base(null, info) { }
    }
}