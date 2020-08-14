using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.AuthenticationRequest
{
    public class AuthenticationRequestPacketSerializer : PacketSerializer<AuthenticationRequestPacket>
    {
        protected override AuthenticationRequestPacket Deserialize(byte[] data) => new AuthenticationRequestPacket();

        protected override byte[] Serialize(AuthenticationRequestPacket item) => new byte[1024];
    }
}