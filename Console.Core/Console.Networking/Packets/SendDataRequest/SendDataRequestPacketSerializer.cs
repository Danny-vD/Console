using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.SendDataRequest
{
    /// <summary>
    /// Serializes SendDataRequestPacket
    /// </summary>
    public class SendDataRequestPacketSerializer : PacketSerializer<SendDataRequestPacket>
    {

        /// <summary>
        /// Deserializes the Data into a Network Packet of Type SendDataRequestPacket
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>SendDataRequestPacket</returns>
        protected override SendDataRequestPacket Deserialize(byte[] data)
        {
            string d = NetworkingSettings.EncodingInstance.GetString(data);
            return new SendDataRequestPacket(d);
        }

        /// <summary>
        /// Serializes the Network Packet of Type SendDataRequestPacket into a Networkable Byte Array
        /// </summary>
        /// <param name="item">SendDataRequestPacket</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(SendDataRequestPacket item)
        {
            return NetworkingSettings.EncodingInstance.GetBytes(item.Destination);
        }

    }
}