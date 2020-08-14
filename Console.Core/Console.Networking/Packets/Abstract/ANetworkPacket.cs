namespace Console.Networking.Packets.Abstract
{
    public abstract class ANetworkPacket
    {
        /// <summary>
        /// Flag that specifies that the networking layer should not encrypt/decrypt the packet.
        /// </summary>
        public virtual bool DoNotEncrypt => false;
        /// <summary>
        /// Unique Identifier for this Packet
        /// </summary>
        public virtual string PacketIdentifier => this.GetType().AssemblyQualifiedName;
    }
}