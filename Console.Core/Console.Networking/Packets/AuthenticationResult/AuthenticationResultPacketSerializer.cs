using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.AuthenticationResult
{
    /// <summary>
    /// PacketSerializer Implementation for AuthenticationResultPacket
    /// </summary>
    public class AuthenticationResultPacketSerializer : PacketSerializer<AuthenticationResultPacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override AuthenticationResultPacket Deserialize(byte[] data)
        {
            return new AuthenticationResultPacket(data[0] == 1);
        }


        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(AuthenticationResultPacket item)
        {
            return new[] {(byte) (item.Success ? 1 : 0)};
        }
    }
}