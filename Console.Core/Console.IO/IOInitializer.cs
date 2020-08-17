using System;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

/// <summary>
/// The IO Extension adds a bunch of commands and features that are useful when working with file systems
/// </summary>
namespace Console.IO
{



    /// <summary>
    /// Initializer of the IO Extension
    /// </summary>
    public class IOInitializer : AExtensionInitializer
    {


        /// <summary>
        /// Version of the IO Extension
        /// </summary>
        [Property("version.io")]
        private static Version IOVersion => Assembly.GetExecutingAssembly().GetName().Version;


        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<IOInitializer>();
            CommandAttributeUtils.AddCommands<IOCommands>();
        }
    }
}
