using Console.Core.ActivationSystem;
using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    [ActivateOn]
    public abstract class APacketHostHandler<T> : IPacketHostHandler
        where T : ANetworkPacket
    {
        public void _Handle(ConsoleSocket client, ANetworkPacket item)
        {
            if (item is T p) Handle(client, p);
            //else throw new InvalidCastException("Expected type: " + typeof(T) + " got: " + item.GetType());
        }
        public abstract void Handle(ConsoleSocket client, T item);
    }
}