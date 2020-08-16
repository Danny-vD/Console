using Console.Core;
using Console.Core.CommandSystem;
using Console.Networking.Commands;

namespace Console.Networking
{
    public class NetworkedConsoleProcess
    {

        private HostConsoleCommand hc = new HostConsoleCommand();
        private ClientConsoleCommand cc = new ClientConsoleCommand();

        public NetworkedConsoleProcess()
        {
            CommandAttributeUtils.AddCommands(hc);
            CommandAttributeUtils.AddCommands(cc);
            AConsoleManager.OnConsoleTick += NetworkingSettings.ClientSession.ProcessLogMessages;
        }
    }
}