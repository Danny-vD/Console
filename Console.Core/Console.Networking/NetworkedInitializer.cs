using Console.Core;

namespace Console.Networking
{
    public class NetworkedInitializer : AExtensionInitializer
    {
        public static NetworkedConsoleProcess Instance { get; private set; }
        public override void Initialize()
        {
            Instance = new NetworkedConsoleProcess();
        }
    }
}