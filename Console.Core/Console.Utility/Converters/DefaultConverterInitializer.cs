using System.Reflection;

using Console.Core.ActivationSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.ConverterSystem;
using Console.Core.PropertySystem;


/// <summary>
/// The DefaultConverters Extension contains a collection of Converters that ease the use of the Command System.
/// </summary>
namespace Console.Utility.Converters
{
    /// <summary>
    /// Initializer of the DefaultConverters Extension
    /// </summary>
    public static class DefaultConverterInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CONVERTER_NAMESPACE = UtilExtensionInitializer.UTIL_NAMESPACE + "::converter";
        internal static void SetUpConverters()
        {
            CommandAttributeUtils.AddCommands(typeof(DefaultConverterInitializer));
            PropertyAttributeUtils.AddProperties(typeof(DefaultConverterInitializer));
            PropertyAttributeUtils.AddProperties<DateTimeConverter>();
            PropertyAttributeUtils.AddProperties<EnumConverter>();
            AConverter[] aco = ActivateOnAttributeUtils.ActivateObjects<AConverter>(Assembly.GetExecutingAssembly());
            foreach (AConverter aConverter in aco)
            {
                CustomConvertManager.AddConverter(aConverter);
            }
        }

    }
}