using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionAbort
{
    /// <summary>
    /// PacketSerializer Implementation for ConnectionAbortPacket
    /// </summary>
    public class ConnectionAbortPacketSerializer : PacketSerializer<ConnectionAbortPacket>
    {

        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override ConnectionAbortPacket Deserialize(byte[] data)
        {
            if (data.Length == 0)
            {
                return new ConnectionAbortPacket();
            }

            return new ConnectionAbortPacket(NetworkingSettings.EncodingInstance.GetString(data));
        }


        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(ConnectionAbortPacket item)
        {
            return item.HasReason ? NetworkingSettings.EncodingInstance.GetBytes(item.Reason) : new byte[0];
        }

    }
}