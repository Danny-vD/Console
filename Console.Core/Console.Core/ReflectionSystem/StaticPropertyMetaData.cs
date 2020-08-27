using System.Reflection;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// MethodMetaData Implementation but without instance variable
    /// </summary>
    public class StaticPropertyMetaData : PropertyMetaData
    {
        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="info">Property used as Backend</param>
        public StaticPropertyMetaData(PropertyInfo info) : base(null, info)
        {
        }
    }
}