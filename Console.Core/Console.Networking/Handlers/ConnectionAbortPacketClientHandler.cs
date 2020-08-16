using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.ConnectionAbort;

namespace Console.Networking.Handlers
{
    public class ConnectionAbortPacketClientHandler : APacketClientHandler<ConnectionAbortPacket>
    {
        public override void Handle(ConnectionAbortPacket item)
        {
            AConsoleManager.Instance.LogWarning("Host aborted the Connection with Reason: " + item.Reason);
            NetworkingSettings.ClientSession.Disconnect();
        }
    }
}