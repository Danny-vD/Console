using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    /// <summary>
    /// Host Side Handler Interface.
    /// </summary>
    public interface IPacketHostHandler
    {
        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="client">The Sending Client.</param>
        /// <param name="item">The Packet</param>
        void _Handle(ConsoleSocket client, ANetworkPacket item);
    }
}