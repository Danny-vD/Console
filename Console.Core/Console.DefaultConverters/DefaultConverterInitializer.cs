using System;
using System.Reflection;
using Console.Core;
using Console.Core.ActivationSystem;
using Console.Core.CommandSystem;
using Console.Core.ConverterSystem;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;


/// <summary>
/// The DefaultConverters Extension contains a collection of Converters that ease the use of the Command System.
/// </summary>
namespace Console.DefaultConverters
{
    /// <summary>
    /// Initializer of the DefaultConverters Extension
    /// </summary>
    public class DefaultConverterInitializer : AExtensionInitializer
    {
        [Property("logs.defaultconverters.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }
        internal static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Prints all Values of an Enum to the Console.
        /// </summary>
        /// <param name="qualifiedName"></param>
        [Command("enum-list", "Lists all Values of the Specified Enum")]
        private static void ListEnumValues(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            if (t != null && t.IsEnum)
            {
                string s = $"Values of Enum: {t.Name}\n";
                Array vals = Enum.GetValues(t);
                foreach (object val in vals)
                {
                    s += "\t" + Enum.GetName(t, val) + "=" + (int) val + "\n";
                }
                Logger.Log(s);
                return;
            }
            Logger.LogWarning("Qualified Name is not recognized or the Type is not an Enum");
        }


        /// <summary>
        /// Version of the DefaultConverters Extension
        /// </summary>
        [Property("version.defaultconverters")]
        private static Version EnvVersion => Assembly.GetExecutingAssembly().GetName().Version;


        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {

            CommandAttributeUtils.AddCommands<DefaultConverterInitializer>();
            PropertyAttributeUtils.AddProperties<DefaultConverterInitializer>();
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