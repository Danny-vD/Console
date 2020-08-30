using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.SendDataRequestResponse
{
    /// <summary>
    /// Serializes SendDataRequestResponsePacket
    /// </summary>
    public class SendDataRequestResponseSerializer : PacketSerializer<SendDataRequestResponsePacket>
    {
        /// <summary>
        /// Serializes a Packet of Type SendDataRequestResponsePacket into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(SendDataRequestResponsePacket item)
        {
            return new[] {(byte) (item.Allowed ? 1 : 0)};
        }

        /// <summary>
        /// Deserializes the Data into a Network Packet of Type SendDataRequestResponsePacket
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override SendDataRequestResponsePacket Deserialize(byte[] data)
        {
            if (data[0] == 1)
            {
                return SendDataRequestResponsePacket.Allow;
            }
            return SendDataRequestResponsePacket.Deny;
        }
    }
}