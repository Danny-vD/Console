using System;

namespace Console.Core.CommandSystem.Builder.EnumAutoFill
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumAutoFillAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type EnumType;
        /// <summary>
        /// 
        /// </summary>
        public EnumAutoFillAttribute() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EnumAutoFillAttribute(Type type)
        {
            EnumType = type;
        }
    }
}