using Console.Core.CommandSystem.Attributes;

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
        [Command(
            "start-console-host",
            HelpMessage = "Creates a Console host at the specified port.",
            Namespace = NetworkingSettings.NETWORKING_HOST_NAMESPACE,
            Aliases = new[] { "start-host" }
        )]
        private void StartHostCommand(int port)
        {
            NetworkingSettings.HostSession.StartHost(port);
        }


        /// <summary>
        /// Stops the Console Host Process
        /// </summary>
        [Command(
            "stop-console-host",
            Namespace = NetworkingSettings.NETWORKING_HOST_NAMESPACE,
            HelpMessage = "Stops the Console Host Server",
            Aliases = new[] { "stop-host" }
        )]
        private void StopHostCommand()
        {
            NetworkingSettings.HostSession.StopHost();
        }

        /// <summary>
        /// Forces the Console Host Process to Abort.
        /// </summary>
        [Command(
            "abort-console-host",
            HelpMessage = "Aborts the Console Host Server",
            Namespace = NetworkingSettings.NETWORKING_HOST_NAMESPACE,
            Aliases = new[] { "abort-host" }
        )]
        private void ForceStopHostCommand()
        {
            NetworkingSettings.HostSession.ForceStopHost();
        }

    }
}