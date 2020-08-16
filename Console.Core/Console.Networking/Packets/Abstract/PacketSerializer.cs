using System;
using Console.Core.ActivationSystem;

namespace Console.Networking.Packets.Abstract
{
    [ActivateOn]
    public abstract class PacketSerializer<T> : IPacketSerializer
    where T : ANetworkPacket
    {
        public Type GetTargetType() => typeof(T);
        public string GetPacketIdentifier() => GetTargetType().AssemblyQualifiedName;

        protected abstract T Deserialize(byte[] data);
        protected abstract byte[] Serialize(T item);
        

        public ANetworkPacket _Deserialize(byte[] data)
        {
            return Deserialize(data);
        }

        public byte[] _Serialize(ANetworkPacket item)
        {
            if (item is T obj)
                return Serialize(obj);
            throw new InvalidCastException("Expected Type: " + typeof(T) + " got: " + item.GetType().Name);
        }
    }
}