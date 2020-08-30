using Console.Core;
using Console.Core.CommandSystem;
using Console.Networking.Commands;

namespace Console.Networking
{
    /// <summary>
    /// Container of the Commands for hosting and connecting
    /// </summary>
    public class NetworkedConsoleProcess
    {
        /// <summary>
        /// Host Commands
        /// </summary>
        private readonly HostConsoleCommand hc = new HostConsoleCommand();
        /// <summary>
        /// Client Commands(Does invoke ProcessLogMessages every ConsoleTick.
        /// </summary>
        private readonly ClientConsoleCommand cc = new ClientConsoleCommand();

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