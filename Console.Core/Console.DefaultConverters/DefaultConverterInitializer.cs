using System;
using Console.Core;
using Console.Core.Commands.ConverterSystem;

namespace Console.DefaultConverters
{
    public class DefaultConverterInitializer:AExtensionInitializer
    {
        public override void Initialize()
        {
            CustomConvertManager.AddConverter(new DateTimeConverter());
            CustomConvertManager.AddConverter(new FileInfoConverter());
            CustomConvertManager.AddConverter(new DirInfoConverter());
            CustomConvertManager.AddConverter(new EnumDigitConverter()); //Before Enum Converter(Enum Converter detects the same conditions)
            CustomConvertManager.AddConverter(new EnumConverter());
        }
    }
}
