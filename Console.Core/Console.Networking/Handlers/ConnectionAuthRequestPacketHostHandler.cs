using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.AuthenticationRequest;

namespace Console.Networking.Handlers
{
    public class ConnectionAuthRequestPacketHostHandler : APacketHostHandler<AuthenticationRequestPacket>
    {
        public override void Handle(ConsoleSocket client, AuthenticationRequestPacket item)
        {
            NetworkingSettings.AuthenticatorInstance.AuthenticateClient(client);
        }
    }
}