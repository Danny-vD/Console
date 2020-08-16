using Console.Core.CommandSystem;

namespace Console.Networking.Commands
{
    public class HostConsoleCommand
    {


        [Command("start-console-host", "Creates a Console host at the specified port.", "start-host")]
        private void StartHostCommand(int port)
        {
            NetworkingSettings.HostSession.StartHost(port);
        }


        [Command("stop-console-host", "Stops the Console Host Server", "stop-host")]
        private void StopHostCommand()
        {
            NetworkingSettings.HostSession.StopHost();
        }

        [Command("abort-console-host", "Aborts the Console Host Server", "abort-host")]
        private void ForceStopHostCommand()
        {
            NetworkingSettings.HostSession.ForceStopHost();
        }
    }
}