using System;
using System.Collections.Generic;
using System.Linq;

using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets
{
    /// <summary>
    /// Static Collection of Serializers.
    /// </summary>
    public static class SerializerCollection
    {

        /// <summary>
        /// All available Serializers
        /// </summary>
        private static readonly List<IPacketSerializer> Serializers = new List<IPacketSerializer>();

        /// <summary>
        /// Internal Function to add a serializer to the system.
        /// </summary>
        /// <param name="serializer">The Serializer to Add</param>
        internal static void AddSerializer(IPacketSerializer serializer)
        {
            Serializers.Add(serializer);
        }

        /// <summary>
        /// Returns the Serializer that matches the Identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private static IPacketSerializer GetSerializer(string identifier)
        {
            return Serializers.FirstOrDefault(x => x.GetPacketIdentifier() == identifier);
        }

        /// <summary>
        /// Returns true when the SerializerCollection contains a serializer that can serialize the class associated to the identifier
        /// </summary>
        /// <param name="identifier">The Class Identifier</param>
        /// <returns>True when serializable</returns>
        public static bool CanSerialize(string identifier)
        {
            return Serializers.Any(x => x.GetPacketIdentifier() == identifier);
        }

        /// <summary>
        /// Returns true when the SerializerCollection contains a serializer that can deserialize the class associated to the identifier
        /// </summary>
        /// <param name="identifier">The Class Identifier</param>
        /// <returns>True when deserializable</returns>
        public static bool CanDeserialize(string identifier)
        {
            return Serializers.Any(x => x.GetPacketIdentifier() == identifier);
        }

        /// <summary>
        /// Deserializes the data with the serializer that is associated with the identifier
        /// </summary>
        /// <typeparam name="T">Target Type</typeparam>
        /// <param name="identifier">Class Identifier</param>
        /// <param name="data">Data to Deserialize</param>
        /// <returns>Deserialized Network Packet</returns>
        public static T Deserialize<T>(string identifier, byte[] data) where T : ANetworkPacket
        {
            byte[] content = new byte[data.Length];
            data.CopyTo(content, 0);
            IPacketSerializer s = GetSerializer(identifier);
            if (s != null)
            {
                return (T) s._Deserialize(content);
            }

            throw new SerializerException("Can not Deserialize: " + identifier);
        }

        /// <summary>
        /// Serializes the Packet with the serializer that is associated with the identifier of the packet.
        /// </summary>
        /// <typeparam name="T">Target Type</typeparam>
        /// <param name="item">Packet to Serialize</param>
        /// <returns>Serialized Network Packet</returns>
        public static byte[] Serialize<T>(T item) where T : ANetworkPacket
        {
            IPacketSerializer s = GetSerializer(item.PacketIdentifier);
            if (s != null)
            {
                return s._Serialize(item);
            }

            throw new SerializerException("Can not Serialize: " + item);
        }

        /// <summary>
        /// Serializer Exception that gets thrown when the SerializerCollection is not able to De/Serialize a Packet.
        /// </summary>
        private class SerializerException : Exception
        {

            /// <summary>
            /// Public Constructor
            /// </summary>
            /// <param name="message">The Exception Message</param>
            public SerializerException(string message) : base(message)
            {
            }

        }

    }
}