using Console.Core.ActivationSystem;
using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    /// <summary>
    /// Host Side Handler Class.
    /// </summary>
    [ActivateOn]
    public abstract class APacketHostHandler<T> : IPacketHostHandler
        where T : ANetworkPacket
    {
        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="client">The Sending Client.</param>
        /// <param name="item">The Packet</param>
        public void _Handle(ConsoleSocket client, ANetworkPacket item)
        {
            if (item is T p) Handle(client, p);
            //else throw new InvalidCastException("Expected type: " + typeof(T) + " got: " + item.GetType());
        }

        /// <summary>
        /// Handles the packet of type T
        /// </summary>
        /// <param name="client">Sending Client</param>
        /// <param name="item">The Packet</param>
        public abstract void Handle(ConsoleSocket client, T item);
    }
}