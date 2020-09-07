using System;
using System.IO;
using System.Reflection;

using Console.Core;
using Console.Core.CommandSystem.Attributes;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;

/// <summary>
/// The PersistentProperties Extension is implementing the Funcionality to Load/Save properties in a human readable format.
/// </summary>
namespace Console.Persistence
{
    /// <summary>
    /// Initializer of the PersistentProperties Extension
    /// </summary>
    public class PersistentPropertiesInitializer : AExtensionInitializer
    {

        /// <summary>
        /// 
        /// </summary>
        public const string PERSISTENT_PROPERTIES_NAMESPACE = ConsoleCoreConfig.PROPERTY_NAMESPACE + "::persistence";

        private const string InitPropertyPath = ".\\configs\\init.cfg";

        [Property("logs.persistence.mute")]
        private static bool MuteLogs
        {
            get => Logger.Mute;
            set => Logger.Mute = value;
        }

        /// <summary>
        /// Logger for this Extension
        /// </summary>
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());


        /// <summary>
        /// Version of the Persistent Properties Extension
        /// </summary>
        [Property("version.persistence")]
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