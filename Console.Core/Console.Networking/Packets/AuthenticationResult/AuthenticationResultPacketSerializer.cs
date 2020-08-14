using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.AuthenticationResult
{
    public class AuthenticationResultPacketSerializer : PacketSerializer<AuthenticationResultPacket>
    {
        protected override AuthenticationResultPacket Deserialize(byte[] data)
        {
            return new AuthenticationResultPacket(data[0] == 1);
        }

        protected override byte[] Serialize(AuthenticationResultPacket item)
        {
            return new [] { (byte)(item.Success ? 1 : 0) };
        }
    }
}