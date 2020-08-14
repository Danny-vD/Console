using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Authentication
{
    public class AuthenticationPacketSerializer : PacketSerializer<AuthenticationPacket>
    {
        protected override AuthenticationPacket Deserialize(byte[] data)
        {
            return new AuthenticationPacket(data);
        }

        protected override byte[] Serialize(AuthenticationPacket item)
        {
            return item.Data;
        }
    }
}