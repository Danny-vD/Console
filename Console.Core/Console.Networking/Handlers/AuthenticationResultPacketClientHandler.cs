using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.AuthenticationResult;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the AuthenticationResultPacket when sent from the Host.
    /// </summary>
    public class AuthenticationResultPacketClientHandler : APacketClientHandler<AuthenticationResultPacket>
    {

        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        public override void Handle(AuthenticationResultPacket item)
        {
            NetworkingSettings.ClientSession.Client.SetAuthenticator(NetworkingSettings.AuthenticatorInstance);
        }

    }
}