using Console.Networking.Packets.Abstract;

/// <summary>
/// SendDataPacket Classes
/// </summary>
namespace Console.Networking.Packets.SendData
{
    /// <summary>
    /// Data Packet for Transmitting Data from the client to the host
    /// </summary>
    public class SendDataPacket : ANetworkPacket
    {
        /// <summary>
        /// Is True when this packet is the last one in this Packet Series
        /// </summary>
        public bool LastPacket;
        /// <summary>
        /// The Data Chunk of the Packet
        /// </summary>
        public byte[] Data;
    }
}