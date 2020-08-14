using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.AuthenticationRequest
{
    /// <summary>
    /// Sent by the Client to Request the Authorization of the Client
    /// </summary>
    public class AuthenticationRequestPacket : ANetworkPacket
    {
        public override bool DoNotEncrypt => true;
    }
}