using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.Authentication;


/// <summary>
/// All Handlers for Handling Packets in the networking Layer.
/// </summary>
namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the AuthenticationPacket when sent from the Host
    /// </summary>
    public class AuthenticationPacketClientHandler : APacketClientHandler<AuthenticationPacket>
    {

        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        public override void Handle(AuthenticationPacket item)
        {
            NetworkingSettings.ClientSession.Client.TrySendPacket(item.CreateResponse());
        }

    }
}