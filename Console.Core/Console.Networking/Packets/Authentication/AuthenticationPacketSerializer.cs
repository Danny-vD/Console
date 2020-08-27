using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Authentication
{
    /// <summary>
    /// PacketSerializer Implementation for AuthenticationPacket
    /// </summary>
    public class AuthenticationPacketSerializer : PacketSerializer<AuthenticationPacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override AuthenticationPacket Deserialize(byte[] data)
        {
            return new AuthenticationPacket(data);
        }

        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(AuthenticationPacket item)
        {
            return item.Data;
        }
    }
}