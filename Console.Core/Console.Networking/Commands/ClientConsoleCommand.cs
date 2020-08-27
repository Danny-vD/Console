using Console.Core.CommandSystem;
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
        [Command("connect-console", "Tries to connect to a hosting console.", "connect")]
        private void ConnectConsoleCommand(string ip, int port)
        {
            NetworkingSettings.ClientSession.Connect(ip, port);
        }

        /// <summary>
        /// Tries to Send a File from the Client to the Host
        /// </summary>
        /// <param name="file">Local File</param>
        /// <param name="destination">Remote Destination</param>
        [Command("send-data", "Sends a File from the Client to the Host.")]
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
        [Command("disconnect-console", "Tries to disconnect from a hosting console", "disconnect")]
        private void DisconnectConsoleCommand()
        {
            NetworkingSettings.ClientSession.Disconnect();
        }

        /// <summary>
        /// Runs a Command on the Remote Console.
        /// </summary>
        /// <param name="command"></param>
        [Command("hrun", "Runs a Remote Command on the connected host.")]
        private void RunHostCommand(string command)
        {
            NetworkingSettings.ClientSession.RunCommand(command);
        }
    }
}