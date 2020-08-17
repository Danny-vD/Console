using System;
using Console.Core.ActivationSystem;

namespace Console.Networking.Packets.Abstract
{
    /// <summary>
    /// Abstract Generic Packet Serializer Implementation.
    /// </summary>
    /// <typeparam name="T">Network Packet Type</typeparam>
    [ActivateOn]
    public abstract class PacketSerializer<T> : IPacketSerializer
    where T : ANetworkPacket
    {
        /// <summary>
        /// Returns the Target Type of the Serializer
        /// </summary>
        /// <returns>Target Type</returns>
        public Type GetTargetType() => typeof(T);
        /// <summary>
        /// Returns the Packet Identifier of the Target Type
        /// </summary>
        /// <returns>The Packet Identifier</returns>
        public string GetPacketIdentifier() => GetTargetType().AssemblyQualifiedName;

        /// <summary>
        /// Deserializes the Data into a Network Packet of Type T
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        protected abstract T Deserialize(byte[] data);
        /// <summary>
        /// Serializes a Packet of Type T into a Byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Data</returns>
        protected abstract byte[] Serialize(T item);


        /// <summary>
        /// Deserializes the Data into a ANetworkPacket
        /// </summary>
        /// <param name="data">Serialized Data</param>
        /// <returns>Network Packet</returns>
        public ANetworkPacket _Deserialize(byte[] data)
        {
            return Deserialize(data);
        }

        /// <summary>
        /// Serializes the Packet into a byte array
        /// </summary>
        /// <param name="item">Packet</param>
        /// <returns>Serialized Packet Data</returns>
        public byte[] _Serialize(ANetworkPacket item)
        {
            if (item is T obj)
                return Serialize(obj);
            throw new InvalidCastException("Expected Type: " + typeof(T) + " got: " + item.GetType().Name);
        }
    }
}