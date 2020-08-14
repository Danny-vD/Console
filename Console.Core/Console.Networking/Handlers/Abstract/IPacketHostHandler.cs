using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    public interface IPacketHostHandler
    {
        void _Handle(ConsoleSocket client, ANetworkPacket item);
    }
}