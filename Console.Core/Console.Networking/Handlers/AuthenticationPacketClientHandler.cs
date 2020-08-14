using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.Authentication;

namespace Console.Networking.Handlers
{
    public class AuthenticationPacketClientHandler : APacketClientHandler<AuthenticationPacket>
    {
        public override void Handle(AuthenticationPacket item)
        {
            NetworkingSettings.ClientSession.Client.TrySendPacket(item.CreateResponse());
        }
    }
}