using System.Collections.Generic;
using System.Linq;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Command
{

    /// <summary>
    /// PacketSerializer Implementation for CommandPacket
    /// </summary>
    public class CommandPacketSerializer : PacketSerializer<CommandPacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override CommandPacket Deserialize(byte[] data)
        {
            return new CommandPacket(data[0] == 1, NetworkingSettings.EncodingInstance.GetString(data, 1, data.Length - 1));
        }

        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(CommandPacket item)
        {
            List<byte> data = NetworkingSettings.EncodingInstance.GetBytes(item.Input).ToList();
            data.Insert(0, (byte)(item.Resolved ? 1 : 0));
            return data.ToArray();
        }
    }
}