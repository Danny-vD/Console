using System;
using System.IO;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

namespace Console.PersistentProperties
{
    public class PersistentPropertiesInitializer : AExtensionInitializer
    {
        private const string InitPropertyPath = ".\\configs\\init.cfg";

        [Property("version.persistentproperties")]
        private static Version PersistentPropertiesVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override LoadOrder Order => LoadOrder.After;

        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<PersistentPropertiesInitializer>();
            CommandAttributeUtils.AddCommands<PropertyLoaderCommands>();
            if (File.Exists(InitPropertyPath))
                PropertyLoaderCommands.Load(InitPropertyPath);
        }
    }
}