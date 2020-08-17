using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Log
{
    /// <summary>
    /// PacketSerializer Implementation for LogPacket
    /// </summary>
    public class LogPacketSerializer : PacketSerializer<LogPacket>
    {
        /// <summary>
        /// Deserializes the packet
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Deserialized Packet</returns>
        protected override LogPacket Deserialize(byte[] data)
        {
            return new LogPacket(NetworkingSettings.EncodingInstance.GetString(data));
        }

        /// <summary>
        /// Serializes the packet
        /// </summary>
        /// <param name="item">The Packet to Serialize</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(LogPacket item)
        {
            return NetworkingSettings.EncodingInstance.GetBytes(item.LogLine);
        }
    }
}