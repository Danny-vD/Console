using System.Reflection;

namespace Console.Core.ReflectionSystem
{
    public class StaticMethodMetaData : MethodMetaData
    {
        public StaticMethodMetaData(MethodInfo info) : base(null, info) { }
    }
}