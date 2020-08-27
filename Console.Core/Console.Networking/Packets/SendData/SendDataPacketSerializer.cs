using System.Collections.Generic;
using System.Linq;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.SendData
{
    /// <summary>
    /// Serializes SendDataPacket s
    /// </summary>
    public class SendDataPacketSerializer : PacketSerializer<SendDataPacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type SendDataPacket
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override SendDataPacket Deserialize(byte[] data)
        {
            bool last = data[0] == 1;
            List<byte> d = data.ToList();
            d.RemoveAt(0);
            return new SendDataPacket {Data = d.ToArray(), LastPacket = last};
        }

        /// <summary>
        /// Serializes the SendDataPacket into a networkable byte array
        /// </summary>
        /// <param name="item">SendDataPacket to Serialize</param>
        /// <returns>Serialized Packet</returns>
        protected override byte[] Serialize(SendDataPacket item)
        {
            List<byte> data = new List<byte>();
            data.Add((byte) (item.LastPacket ? 1 : 0));
            data.AddRange(item.Data);
            return data.ToArray();
        }
    }
}