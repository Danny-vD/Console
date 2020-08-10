using System;
using System.Reflection;
using Console.Core;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.ArrayConverter
{
    public class ArrayConverterInitializer : AExtensionInitializer
    {
        [Property("version.arrayconverter")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ArrayConverterInitializer>();
            CustomConvertManager.AddConverter(new ArrayCustomConverter());
        }
    }
}