using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    public interface IPacketClientHandler
    {
        void _Handle(ANetworkPacket item);
    }
}