using System;

namespace Console.Core.CommandSystem.Builder.EnumAutoFill
{

    /// <summary>
    /// When a Parameter Gets Decorated with the EnumAutoFillAttribute it enables the CommandBuilder to Suggest Possible Enum Name Entry Values for this Parameter
    /// </summary>
    public class EnumAutoFillAttribute : ConsoleAttribute
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