using System;

namespace Console.Networking.Packets.Abstract
{
    internal interface IPacketSerializer
    {
        Type GetTargetType();
        string GetPacketIdentifier();
        ANetworkPacket _Deserialize(byte[] data);
        byte[] _Serialize(ANetworkPacket item);
    }
}