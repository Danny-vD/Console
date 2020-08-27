using Console.Core.CommandSystem;

namespace Console.Networking.Commands
{
    /// <summary>
    /// Host Side Commands
    /// </summary>
    public class HostConsoleCommand
    {
        /// <summary>
        /// Starts the Host Process on the Specified port
        /// </summary>
        /// <param name="port">Host Port</param>
        [Command("start-console-host", "Creates a Console host at the specified port.", "start-host")]
        private void StartHostCommand(int port)
        {
            NetworkingSettings.HostSession.StartHost(port);
        }


        /// <summary>
        /// Stops the Console Host Process
        /// </summary>
        [Command("stop-console-host", "Stops the Console Host Server", "stop-host")]
        private void StopHostCommand()
        {
            NetworkingSettings.HostSession.StopHost();
        }

        /// <summary>
        /// Forces the Console Host Process to Abort.
        /// </summary>
        [Command("abort-console-host", "Aborts the Console Host Server", "abort-host")]
        private void ForceStopHostCommand()
        {
            NetworkingSettings.HostSession.ForceStopHost();
        }
    }
}