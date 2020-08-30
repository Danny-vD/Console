using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.AuthenticationRequest
{
    /// <summary>
    /// PacketSerializer Implementation for AuthenticationRequestPacket
    /// </summary>
    public class AuthenticationRequestPacketSerializer : PacketSerializer<AuthenticationRequestPacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override AuthenticationRequestPacket Deserialize(byte[] data)
        {
            return new AuthenticationRequestPacket();
        }


        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(AuthenticationRequestPacket item)
        {
            return new byte[1024];
        }
    }
}