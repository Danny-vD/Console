using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.ConnectionRequest;
using Console.Networking.Packets.ConnectionResponse;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the ConnectionRequestPacket when sent from a Client.
    /// </summary>
    public class ConnectionRequestHostHandler : APacketHostHandler<ConnectionRequestPacket>
    {

        /// <summary>
        /// Handles the packet of type T
        /// </summary>
        /// <param name="client">Sending Client</param>
        /// <param name="item">The Packet</param>
        public override void Handle(ConsoleSocket client, ConnectionRequestPacket item)
        {
            ConnectionRequestResponsePacket ret = null;
            if (item.Version != NetworkingSettings.NetworkVersion.ToString())
                ret = new ConnectionRequestResponseFailedPacket("Version Mismatch");
            else ret = new ConnectionRequestResponseSuccessPacket(NetworkingSettings.Authenticator);
            if (!client.TrySendPacket(ret))
            {
                NetworkedInitializer.Logger.LogWarning("Can not send Packet " + item);
            }
        }
    }
}