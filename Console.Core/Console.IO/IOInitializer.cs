using Console.Core;
using Console.Core.Utils;

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
