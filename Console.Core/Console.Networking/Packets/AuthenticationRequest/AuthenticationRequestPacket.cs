using Console.Networking.Packets.Abstract;


/// <summary>
/// AuthenticationRequestPacket Classes
/// </summary>
namespace Console.Networking.Packets.AuthenticationRequest
{
    /// <summary>
    /// Sent by the Client to Request the Authorization of the Client
    /// </summary>
    public class AuthenticationRequestPacket : ANetworkPacket
    {
        /// <summary>
        /// Flag that specifies that the networking layer should not encrypt/decrypt the packet.
        /// </summary>
        public override bool DoNotEncrypt => true;
    }
}