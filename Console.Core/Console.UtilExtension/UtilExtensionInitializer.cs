using System;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;


/// <summary>
/// The UtilExtension Extension is used as container for Useful Commands that are unsafe or not feasible to be embedded in the core library.
/// </summary>
namespace Console.UtilExtension
{
    /// <summary>
    /// Initializer of the UtilExtension Extension
    /// </summary>
    public class UtilExtensionInitializer : AExtensionInitializer
    {
        /// <summary>
        /// Version of the UtilExtension Extension
        /// </summary>
        [Property("version.utils")]
        private static Version UtilsVersion => Assembly.GetExecutingAssembly().GetName().Version;


        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<UtilExtensionInitializer>();
            CommandAttributeUtils.AddCommands<UtilCommandCommands>();
            CommandAttributeUtils.AddCommands<UtilPropertyCommands>();
        }
    }
}