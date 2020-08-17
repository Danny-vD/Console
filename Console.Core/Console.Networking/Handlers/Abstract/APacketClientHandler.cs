using Console.Core.ActivationSystem;
using Console.Networking.Packets.Abstract;


/// <summary>
/// Abstract Packet Handler Implementations
/// </summary>
namespace Console.Networking.Handlers.Abstract
{
    /// <summary>
    /// Client Side Handler Class.
    /// </summary>
    [ActivateOn]
    public abstract class APacketClientHandler<T> : IPacketClientHandler
        where T : ANetworkPacket
    {

        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        public void _Handle(ANetworkPacket item)
        {
            if (item is T p) Handle(p);
            //else throw new InvalidCastException("Expected type: " + typeof(T) + " got: " + item.GetType());
        }

        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        public abstract void Handle(T item);
    }
}