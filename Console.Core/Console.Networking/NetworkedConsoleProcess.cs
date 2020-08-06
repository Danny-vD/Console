using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Console;

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
            AConsoleManager.OnConsoleTick += HostConsoleCommand.ProcessQueue;
            AConsoleManager.OnConsoleTick += ClientConsoleCommand.ProcessLogMessages;
        }
    }
}