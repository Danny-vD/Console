using Console.Core;
using Console.Core.Attributes.CommandSystem.Helper;

namespace Console.PersistentProperties
{
    public class PersistentPropertiesInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<PropertyLoaderCommands>();
        }
    }
}