using System.Reflection;

namespace Console.Core.ReflectionSystem
{
    /// <summary>
    /// FieldMetaData Implementation but without instance variable
    /// </summary>
    public class StaticFieldMetaData : FieldMetaData
    {

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="info">FieldInfo used as Backend</param>
        public StaticFieldMetaData(FieldInfo info) : base(null, info)
        {
        }

    }
}