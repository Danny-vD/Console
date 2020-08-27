using System.Collections.Generic;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionResponse
{
    /// <summary>
    /// PacketSerializer Implementation for ConnectionRequestResponsePacket and subclasses.
    /// </summary>
    public class ConnectionRequestResponseSerializer : PacketSerializer<ConnectionRequestResponsePacket>
    {
        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected override ConnectionRequestResponsePacket Deserialize(byte[] data)
        {
            bool suc = data[0] == 1;
            return suc
                ? (ConnectionRequestResponsePacket) new ConnectionRequestResponseSuccessPacket(
                    NetworkingSettings.EncodingInstance.GetString(data, 1, data.Length - 1))
                : (ConnectionRequestResponsePacket) new ConnectionRequestResponseFailedPacket(
                    NetworkingSettings.EncodingInstance.GetString(data, 1, data.Length - 1));
        }


        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected override byte[] Serialize(ConnectionRequestResponsePacket item)
        {
            List<byte> data = new List<byte> {(byte) (item.Success ? 1 : 0)};
            if (item is ConnectionRequestResponseFailedPacket fail)
            {
                data.AddRange(NetworkingSettings.EncodingInstance.GetBytes(fail.Reason));
            }
            if (item is ConnectionRequestResponseSuccessPacket suc)
            {
                data.AddRange(NetworkingSettings.EncodingInstance.GetBytes(suc.AuthMethod));
            }

            return data.ToArray();
        }
    }
}