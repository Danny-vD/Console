using Console.Core.Attributes.CommandSystem;

namespace Console.Networking.Commands
{
    public class ClientConsoleCommand
    {

        [Command("connect-console", "Tries to connect to a hosting console.", "connect")]
        private void ConnectConsoleCommand(string ip, int port)
        {
            NetworkingSettings.ClientSession.Connect(ip, port);
        }

        

        [Command("disconnect-console", "Tries to disconnect from a hosting console", "disconnect")]
        private void DisconnectConsoleCommand()
        {
            NetworkingSettings.ClientSession.Disconnect();
        }

        [Command("hrun", "Runs a Remote Command on the connected host.")]
        private void RunHostCommand(string command)
        {
            NetworkingSettings.ClientSession.RunCommand(command);
        }
    }
}