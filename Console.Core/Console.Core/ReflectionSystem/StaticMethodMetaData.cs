using System.Reflection;

namespace Console.Core.ReflectionSystem
{

    /// <summary>
    /// MethodMetaData Implementation but without instance variable
    /// </summary>
    public class StaticMethodMetaData : MethodMetaData
    {
        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="info">MethodInfo used as Backend</param>
        public StaticMethodMetaData(MethodInfo info) : base(null, info) { }
    }
}