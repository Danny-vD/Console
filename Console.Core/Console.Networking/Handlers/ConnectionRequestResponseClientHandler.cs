using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Console.Core.Console;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.AuthenticationRequest;
using Console.Networking.Packets.ConnectionResponse;

namespace Console.Networking.Handlers
{
    public class ConnectionRequestResponseClientHandler : APacketClientHandler<ConnectionRequestResponsePacket>
    {
        public override void Handle(ConnectionRequestResponsePacket packet)
        {
            if (packet is ConnectionRequestResponseFailedPacket failedPacket)
            {
                AConsoleManager.Instance.LogWarning("Host denied connection: " + failedPacket.Reason);
            }
            else if (packet is ConnectionRequestResponseSuccessPacket successPacket)
            {
                NetworkingSettings.Authenticator = successPacket.AuthMethod;
                if (NetworkingSettings.AuthenticatorInstance == null) //Authenticator not found
                {
                    AConsoleManager.Instance.LogWarning("Authenticator with the name: " + NetworkingSettings.Authenticator + " does not exist.");
                }
                else
                {
                    AuthenticationRequestPacket car = new AuthenticationRequestPacket();
                    if (!NetworkingSettings.ClientSession.Client.TrySendPacket(car))
                    {
                        AConsoleManager.Instance.LogWarning("Can not send packet: " + car.PacketIdentifier);
                    }
                    // NetworkingSettings.AuthenticatorInstance.AuthenticateClient(NetworkingSettings.ClientSession.Client);
                    //Authenticate
                }
            }
        }

    }
}