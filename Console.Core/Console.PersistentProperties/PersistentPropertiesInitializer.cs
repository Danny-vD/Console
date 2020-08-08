using System.IO;
using Console.Core;
using Console.Core.Utils;

namespace Console.PersistentProperties
{
    public class PersistentPropertiesInitializer : AExtensionInitializer
    {
        public override LoadOrder Order => LoadOrder.After;
        private const string InitPropertyPath = ".\\configs\\init.cfg";

        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<PropertyLoaderCommands>();
            if (File.Exists(InitPropertyPath))
                PropertyLoaderCommands.Load(InitPropertyPath);
        }
    }
}