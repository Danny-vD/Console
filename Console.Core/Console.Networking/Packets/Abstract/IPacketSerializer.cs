using System;

namespace Console.Networking.Packets.Abstract
{
    /// <summary>
    /// PacketSerializer Interface.
    /// </summary>
    internal interface IPacketSerializer
    {

        /// <summary>
        /// Returns the Target Type of the Serializer
        /// </summary>
        /// <returns>Target Type</returns>
        Type GetTargetType();

        /// <summary>
        /// Returns the Packet Identifier of the Target Type
        /// </summary>
        /// <returns>The Packet Identifier</returns>
        string GetPacketIdentifier();

        /// <summary>
        /// Deserializes the Data into a ANetworkPacket
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        ANetworkPacket _Deserialize(byte[] data);

        /// <summary>
        /// Serializes the Packet into a byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Packet Data</returns>
        byte[] _Serialize(ANetworkPacket item);

    }
}