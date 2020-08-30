using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.ConnectionAbort;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the ConnectionAbortPacket when sent from the Host.
    /// </summary>
    public class ConnectionAbortPacketClientHandler : APacketClientHandler<ConnectionAbortPacket>
    {
        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        public override void Handle(ConnectionAbortPacket item)
        {
            NetworkedInitializer.Logger.Log("Host aborted the Connection with Reason: " + item.Reason);
            NetworkingSettings.ClientSession.Disconnect();
        }
    }
}