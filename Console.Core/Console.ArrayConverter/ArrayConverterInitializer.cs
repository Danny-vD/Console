using System;
using System.Reflection;
using Console.Core.ConverterSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

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