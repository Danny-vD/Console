using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    /// <summary>
    /// Client Side Handler Interface.
    /// </summary>
    public interface IPacketClientHandler
    {

        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        void _Handle(ANetworkPacket item);

    }
}