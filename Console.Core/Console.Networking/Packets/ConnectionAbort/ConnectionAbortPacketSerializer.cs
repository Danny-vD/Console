using System.Text;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionAbort
{
    public class ConnectionAbortPacketSerializer : PacketSerializer<ConnectionAbortPacket>
    {
        protected override ConnectionAbortPacket Deserialize(byte[] data)
        {
            if(data.Length==0)return new ConnectionAbortPacket();
            return new ConnectionAbortPacket(NetworkingSettings.EncodingInstance.GetString(data));
        }

        protected override byte[] Serialize(ConnectionAbortPacket item)
        {
            return item.HasReason ? NetworkingSettings.EncodingInstance.GetBytes(item.Reason) : new byte[0];
        }
    }
}