using System.Collections;
using Console.Core.Attributes.CommandSystem.Helper;
using Console.Core.Attributes.PropertySystem;
using Console.Core.Commands;
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
            AConsoleManager.OnConsoleTick += HostRoutine;
            AConsoleManager.OnConsoleTick += ClientRoutine;
        }


        private void HostRoutine()
        {
            HostConsoleCommand.ProcessQueue();
            //yield return new WaitForSeconds(HostProcessingTick);

        }

        private void ClientRoutine()
        {
            ClientConsoleCommand.ProcessLogMessages();
            //yield return new WaitForSeconds(ClientLogPollingTick);

        }
    }
}