using System;
using System.IO;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

/// <summary>
/// The PersistentProperties Extension is implementing the Funcionality to Load/Save properties in a human readable format.
/// </summary>
namespace Console.PersistentProperties
{
    /// <summary>
    /// Initializer of the PersistentProperties Extension
    /// </summary>
    public class PersistentPropertiesInitializer : AExtensionInitializer
    {
        [Property("logs.persistentproperties.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        /// <summary>
        /// Logger for this Extension
        /// </summary>
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());
        private const string InitPropertyPath = ".\\configs\\init.cfg";


        /// <summary>
        /// Version of the Persistent Properties Extension
        /// </summary>
        [Property("version.persistentproperties")]
        private static Version PersistentPropertiesVersion => Assembly.GetExecutingAssembly().GetName().Version;



        /// <summary>
        /// The Load Order of the Extension
        /// </summary>
        public override LoadOrder Order => LoadOrder.After;


        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<PersistentPropertiesInitializer>();
            CommandAttributeUtils.AddCommands<PropertyLoaderCommands>();
            if (File.Exists(InitPropertyPath))
            {
                PropertyLoaderCommands.Load(InitPropertyPath);
            }
        }
    }
}