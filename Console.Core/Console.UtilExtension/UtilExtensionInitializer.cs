using System;
using System.Reflection;
using Console.Core;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.UtilExtension
{
    public class UtilExtensionInitializer : AExtensionInitializer
    {

        [Property("version.utils")]
        private static Version UtilsVersion => Assembly.GetExecutingAssembly().GetName().Version;

        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<UtilExtensionInitializer>();
            CommandAttributeUtils.AddCommands<UtilCommandCommands>();
            CommandAttributeUtils.AddCommands<UtilPropertyCommands>();
        }
    }
}
