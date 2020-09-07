using Console.Core.CommandSystem.Attributes;
using Console.Core.CommandSystem.Builder.BuiltIn.CommandAutoFill;
using Console.Networking.SendData;

/// <summary>
/// Commands used to control the Networking System
/// </summary>
namespace Console.Networking.Commands
{
    /// <summary>
    /// Client Side Commands.
    /// </summary>
    public class ClientConsoleCommand
    {

        /// <summary>
        /// Tries to Connect to a hosting console.
        /// </summary>
        /// <param name="ip">Host IP</param>
        /// <param name="port">Host Port</param>
        [Command(
            "connect-console",
            HelpMessage = "Tries to connect to a hosting console.",
            Namespace = NetworkingSettings.NETWORKING_CLIENT_NAMESPACE,
            Aliases = new[] { "connect" }
        )]
        private void ConnectConsoleCommand(string ip, int port)
        {
            NetworkingSettings.ClientSession.Connect(ip, port);
        }

        /// <summary>
        /// Tries to Send a File from the Client to the Host
        /// </summary>
        /// <param name="file">Local File</param>
        /// <param name="destination">Remote Destination</param>
        [Command(
            "send-data",
            Namespace = NetworkingSettings.NETWORKING_CLIENT_NAMESPACE,
            HelpMessage = "Sends a File from the Client to the Host."
        )]
        private void TrySendData(string file, string destination)
        {
            if (NetworkingSettings.ClientSession.Client != null &&
                NetworkingSettings.ClientSession.Client.IsAuthenticated &&
                NetworkingSettings.ClientSession.Client.Connected)
            {
                SendDataManager.TrySendData(file, destination);
            }
        }


        /// <summary>
        /// Tries to Disconnect from a hosting console.
        /// </summary>
        [Command(
            "disconnect-console",
            HelpMessage = "Tries to disconnect from a hosting console",
            Namespace = NetworkingSettings.NETWORKING_CLIENT_NAMESPACE,
            Aliases = new[] { "disconnect" }
        )]
        private void DisconnectConsoleCommand()
        {
            NetworkingSettings.ClientSession.Disconnect();
        }

        /// <summary>
        /// Runs a Command on the Remote Console.
        /// </summary>
        /// <param name="command"></param>
        [Command(
            "hrun",
            Namespace = NetworkingSettings.NETWORKING_CLIENT_NAMESPACE,
            HelpMessage = "Runs a Remote Command on the connected host."
        )]
        private void RunHostCommand(
            [CommandAutoFill]
            string command)
        {
            NetworkingSettings.ClientSession.RunCommand(command);
        }

    }
}