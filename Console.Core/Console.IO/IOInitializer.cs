using System;
using System.Reflection;
using Console.Core.CommandSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;

namespace Console.IO
{
    public class IOInitializer : AExtensionInitializer
    {
        [Property("version.io")]
        private static Version IOVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<IOInitializer>();
            CommandAttributeUtils.AddCommands<IOCommands>();
        }
    }
}
