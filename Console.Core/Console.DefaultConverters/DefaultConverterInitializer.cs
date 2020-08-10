using System;
using System.Reflection;
using Console.Core;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.DefaultConverters
{
    public class DefaultConverterInitializer:AExtensionInitializer
    {
        [Property("version.defaultconverters")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<DefaultConverterInitializer>();
            PropertyAttributeUtils.AddProperties<DateTimeConverter>();
            PropertyAttributeUtils.AddProperties<EnumConverter>();
            CustomConvertManager.AddConverter(new DateTimeConverter());
            CustomConvertManager.AddConverter(new FileInfoConverter());
            CustomConvertManager.AddConverter(new DirInfoConverter());
            CustomConvertManager.AddConverter(new EnumDigitConverter()); //Before Enum Converter(Enum Converter detects the same conditions)
            CustomConvertManager.AddConverter(new EnumConverter());
        }
    }
}
