using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets
{

    public static class SerializerCollection
    {
        private static readonly List<IPacketSerializer> Serializers = new List<IPacketSerializer>();

        internal static void AddSerializer(IPacketSerializer serializer)
        {
            Serializers.Add(serializer);
        }

        private static IPacketSerializer GetSerializer(string identifier) =>
            Serializers.FirstOrDefault(x => x.GetPacketIdentifier() == identifier);

        public static bool CanSerialize(string identifier) => Serializers.Any(x => x.GetPacketIdentifier() == identifier);
        public static bool CanDeserialize(string identifier) => Serializers.Any(x => x.GetPacketIdentifier() == identifier);

        public static T Deserialize<T>(string identifier, byte[] data) where T : ANetworkPacket
        {
            byte[] content = new byte[data.Length];
            data.CopyTo(content,0);
            IPacketSerializer s = GetSerializer(identifier);
            if (s != null)
            {
                return (T)s._Deserialize(content);
            }
            throw new SerializerException("Can not Deserialize: " + identifier);
        }

        public static byte[] Serialize<T>(T item) where T : ANetworkPacket
        {
            IPacketSerializer s = GetSerializer(item.PacketIdentifier);
            if (s != null)
            {
                return s._Serialize(item);
            }
            throw new SerializerException("Can not Serialize: " + item);
        }

        private class SerializerException : Exception
        {
            public SerializerException(string message) : base(message) { }
        }

        private static string ReadIdentifier(byte[] data, out int position)
        {
            int len = BitConverter.ToInt32(data, 0);
            position = len + sizeof(int);
            return NetworkingSettings.EncodingInstance.GetString(data, sizeof(int), len);
        }
    }
}