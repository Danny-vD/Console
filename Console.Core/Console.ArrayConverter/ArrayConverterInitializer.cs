using System;
using System.Reflection;
using Console.Core.ConverterSystem;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
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
        [Property("arrayconverter.logs.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Version of the ArrayConverter Extension
        /// </summary>
        [Property("version.arrayconverter")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ArrayConverterInitializer>();
            CustomConvertManager.AddConverter(new ArrayCustomConverter());
        }
    }
}