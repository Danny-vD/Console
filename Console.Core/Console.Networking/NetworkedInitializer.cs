using System;
using System.Reflection;
using Console.Core;
using Console.Core.PropertySystem;
using Console.Core.Utils;

namespace Console.Networking
{
    public class NetworkedInitializer : AExtensionInitializer
    {
        [Property("version.networking")]
        private static Version NetworkVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public static NetworkedConsoleProcess Instance { get; private set; }
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties<NetworkedInitializer>();
            PropertyAttributeUtils.AddProperties<HostConsoleCommand>();
            PropertyAttributeUtils.AddProperties<ClientConsoleCommand>();
            Instance = new NetworkedConsoleProcess();
        }
    }
}