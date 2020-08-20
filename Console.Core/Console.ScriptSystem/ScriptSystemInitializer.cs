using System;
using System.Reflection;
using Console.Core.ActivationSystem;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.ScriptSystem.Deblocker;
using Console.ScriptSystem.Deblocker.Implementations;

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
            CommandAttributeUtils.AddCommands(typeof(SequenceSystem));
            PropertyAttributeUtils.AddPropertiesByType(typeof(ScriptSystem));
            PropertyAttributeUtils.AddPropertiesByType(typeof(DeblockerSettings));

            ADeblocker[] db = ActivateOnAttributeUtils.ActivateObjects<ADeblocker>(Assembly.GetExecutingAssembly());
            foreach (ADeblocker aDeblocker in db)
            {
                if (aDeblocker.GetType() != typeof(DefaultDeblocker))
                    DeblockerCollection.AddDeblocker(aDeblocker);
            }
        }

    }
}
