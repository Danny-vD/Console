using Console.Core;
using Console.Core.CommandSystem.Attributes;
using Console.Networking.Commands;

namespace Console.Networking
{
    /// <summary>
    /// Container of the Commands for hosting and connecting
    /// </summary>
    public class NetworkedConsoleProcess
    {

        /// <summary>
        /// Client Commands(Does invoke ProcessLogMessages every ConsoleTick.
        /// </summary>
        private readonly ClientConsoleCommand cc = new ClientConsoleCommand();

        /// <summary>
        /// Host Commands
        /// </summary>
        private readonly HostConsoleCommand hc = new HostConsoleCommand();

        /// <summary>
        /// Public Constructor
        /// </summary>
        public NetworkedConsoleProcess()
        {
            CommandAttributeUtils.AddCommands(hc);
            CommandAttributeUtils.AddCommands(cc);
            AConsoleManager.OnConsoleTick += NetworkingSettings.ClientSession.ProcessLogMessages;
        }

    }
}