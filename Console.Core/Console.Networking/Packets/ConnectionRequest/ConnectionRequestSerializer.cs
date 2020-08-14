using System.Text;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionRequest
{
    public class ConnectionRequestSerializer : PacketSerializer<ConnectionRequestPacket>
    {
        protected override ConnectionRequestPacket Deserialize(byte[] data)
        {
            return new ConnectionRequestPacket(NetworkingSettings.EncodingInstance.GetString(data));
        }

        protected override byte[] Serialize(ConnectionRequestPacket item)
        {
            byte[] data= NetworkingSettings.EncodingInstance.GetBytes(item.Version);
            return data;
        }
    }
}