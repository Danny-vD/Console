using Console.Core;
using Console.Core.Attributes.CommandSystem.Helper;

namespace Console.IO
{
    public class IOInitializer : AExtensionInitializer
    {
        public override void Initialize()
        {
            CommandAttributeUtils.AddCommands<IOCommands>();
        }
    }
}
