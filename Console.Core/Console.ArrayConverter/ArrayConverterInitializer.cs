using System;
using System.Reflection;
using Console.Core.ConverterSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

/// <summary>
/// Array Converter Extension is used to automatically convert from any type to an array or list of this type.
/// </summary>
namespace Console.ArrayConverter
{
    /// <summary>
    /// Initializer of the ArrayConverter Extension
    /// </summary>
    public class ArrayConverterInitializer : AExtensionInitializer
    {

        /// <summary>
        /// Version of the ArrayConverter Extension
        /// </summary>
        [Property("version.arrayconverter")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ArrayConverterInitializer>();
            CustomConvertManager.AddConverter(new ArrayCustomConverter());
        }
    }
}