using Console.Core.Console;
using Console.Core.Utils;

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