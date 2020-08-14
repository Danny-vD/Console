using Console.Networking.Packets.Abstract;

namespace Console.Networking.Handlers.Abstract
{
    public abstract class APacketClientHandler<T> : IPacketClientHandler
        where T : ANetworkPacket
    {
        public void _Handle(ANetworkPacket item)
        {
            if (item is T p) Handle(p);
            //else throw new InvalidCastException("Expected type: " + typeof(T) + " got: " + item.GetType());
        }
        public abstract void Handle(T item);
    }
}