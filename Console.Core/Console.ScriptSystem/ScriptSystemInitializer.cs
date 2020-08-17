using System;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

/// <summary>
/// The Script System Extension implements functionality that allows running of scripts as if they were typed in the console input line by line.
/// </summary>
namespace Console.ScriptSystem
{



    /// <summary>
    /// Initializer of the ScriptSystem Extension
    /// </summary>
    public class ScriptSystemInitializer : AExtensionInitializer
    {
        
        /// <summary>
        /// Version of the ScriptSystem Extension
        /// </summary>
        [Property("version.scriptsystem")]
        private static Version ScriptSystemVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Initialization Function
        /// </summary>
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<ScriptSystemInitializer>();
            CommandAttributeUtils.AddCommands(typeof(ScriptSystem));
            PropertyAttributeUtils.AddPropertiesByType(typeof(ScriptSystem));
        }
        
    }
}
