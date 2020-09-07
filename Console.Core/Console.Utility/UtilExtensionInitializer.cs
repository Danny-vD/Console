using System;
using System.Reflection;

using Console.Core.ActivationSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Builder;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;
using Console.Utility.Commands.Commands;
using Console.Utility.Commands.Properties;
using Console.Utility.Converters;


/// <summary>
/// The UtilExtension Extension is used as container for Useful Commands that are unsafe or not feasible to be embedded in the core library.
/// </summary>
namespace Console.Utility
{
    /// <summary>
    /// Initializer of the UtilExtension Extension
    /// </summary>
    public class UtilExtensionInitializer : AExtensionInitializer
    {

        private static ALogger Logger = TypedLogger.CreateTypedWithPrefix(UTIL_NAMESPACE);
        /// <summary>
        /// 
        /// </summary>
        public const string UTIL_NAMESPACE = "utility";

        /// <summary>
        /// Version of the UtilExtension Extension
        /// </summary>
        [Property("version."+UTIL_NAMESPACE)]
        private static Version UtilsVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<UtilExtensionInitializer>();
            CommandAttributeUtils.AddCommands<UtilCommandCommands>();
            CommandAttributeUtils.AddCommands<UtilExtensionInitializer>();
            CommandAttributeUtils.AddCommands<UtilPropertyCommands>();


            AutoFillProvider[] provs =
                ActivateOnAttributeUtils.ActivateObjects<AutoFillProvider>(Assembly.GetExecutingAssembly());

            CommandBuilder.AddProvider(provs);

            DefaultConverterInitializer.SetUpConverters();
        }
        /// <summary>
        /// Prints all Values of an Enum to the Console.
        /// </summary>
        /// <param name="qualifiedName"></param>
        [Command("enum-list", HelpMessage = "Lists all Values of the Specified Enum", Namespace = UTIL_NAMESPACE)]
        private static void ListEnumValues(string qualifiedName)
        {
            Type t = Type.GetType(qualifiedName);
            if (t != null && t.IsEnum)
            {
                string s = $"Values of Enum: {t.Name}\n";
                Array vals = Enum.GetValues(t);
                foreach (object val in vals)
                {
                    s += "\t" + Enum.GetName(t, val) + "=" + (int)val + "\n";
                }

                Logger.Log(s);
                return;
            }

            Logger.LogWarning("Qualified Name is not recognized or the Type is not an Enum");
        }
    }
}