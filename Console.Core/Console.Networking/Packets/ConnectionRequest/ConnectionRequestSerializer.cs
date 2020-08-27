using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionRequest
{
    /// <summary>
    /// PacketSerializer Implementation for ConnectionRequestPacket
    /// </summary>
    public class ConnectionRequestSerializer : PacketSerializer<ConnectionRequestPacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override ConnectionRequestPacket Deserialize(byte[] data)
        {
            return new ConnectionRequestPacket(NetworkingSettings.EncodingInstance.GetString(data));
        }

        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(ConnectionRequestPacket item)
        {
            byte[] data = NetworkingSettings.EncodingInstance.GetBytes(item.Version);
            return data;
        }
    }
}