namespace Console.Networking.Packets.ConnectionResponse
{

    /// <summary>
    /// Gets sent if the Host has denied the Connection
    /// </summary>
    public class ConnectionRequestResponseFailedPacket : ConnectionRequestResponsePacket
    {
        public override bool Success => false;
        public override string PacketIdentifier => typeof(ConnectionRequestResponsePacket).AssemblyQualifiedName;
        /// <summary>
        /// The Reason why the Connection Request is Denied.
        /// </summary>
        public readonly string Reason;

        public ConnectionRequestResponseFailedPacket(string reason = null)
        {
            Reason = reason ?? "";
        }
    }
}