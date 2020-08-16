using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.AuthenticationResult;

namespace Console.Networking.Handlers
{
    public class AuthenticationResultPacketHandler : APacketClientHandler<AuthenticationResultPacket>
    {
        public override void Handle(AuthenticationResultPacket item)
        {
            NetworkingSettings.ClientSession.Client.SetAuthenticator(NetworkingSettings.AuthenticatorInstance);
        }
    }
}