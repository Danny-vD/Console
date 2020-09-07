using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.ConnectionAbort;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the ConnectionAbortPacket when sent from the Client.
    /// </summary>
    public class ConnectionAbortPacketHostHandler : APacketHostHandler<ConnectionAbortPacket>
    {

        /// <summary>
        /// Handles the packet of type T
        /// </summary>
        /// <param name="client">Sending Client</param>
        /// <param name="item">The Packet</param>
        public override void Handle(ConsoleSocket client, ConnectionAbortPacket item)
        {
            NetworkedInitializer.Logger.LogWarning("Client aborted the Connection with Reason: " + item.Reason);
            NetworkingSettings.HostSession.RemoveClient(client);
        }

    }
}