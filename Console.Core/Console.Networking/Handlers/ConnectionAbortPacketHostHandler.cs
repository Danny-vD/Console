using Console.Core.Console;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.ConnectionAbort;

namespace Console.Networking.Handlers
{
    public class ConnectionAbortPacketHostHandler : APacketHostHandler<ConnectionAbortPacket>
    {
        public override void Handle(ConsoleSocket client, ConnectionAbortPacket item)
        {
            AConsoleManager.Instance.LogWarning("Client aborted the Connection with Reason: " + item.Reason);
            NetworkingSettings.HostSession.RemoveClient(client);
        }
    }
}