using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Log
{
    public class LogPacketSerializer : PacketSerializer<LogPacket>
    {
        protected override LogPacket Deserialize(byte[] data)
        {
            return new LogPacket(NetworkingSettings.EncodingInstance.GetString(data));
        }

        protected override byte[] Serialize(LogPacket item)
        {
            return NetworkingSettings.EncodingInstance.GetBytes(item.LogLine);
        }
    }
}