using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.AuthenticationRequest;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the AuthenticationRequestPacket when sent from the Client.
    /// </summary>
    public class ConnectionAuthRequestPacketHostHandler : APacketHostHandler<AuthenticationRequestPacket>
    {
        /// <summary>
        /// Handles the packet of type T
        /// </summary>
        /// <param name="client">Sending Client</param>
        /// <param name="item">The Packet</param>
        public override void Handle(ConsoleSocket client, AuthenticationRequestPacket item)
        {
            NetworkingSettings.AuthenticatorInstance.AuthenticateClient(client);
        }
    }
}