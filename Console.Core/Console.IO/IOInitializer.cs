using System;
using System.Reflection;
using Console.Core;
using Console.Core.PropertySystem;
using Console.Core.Utils;

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
