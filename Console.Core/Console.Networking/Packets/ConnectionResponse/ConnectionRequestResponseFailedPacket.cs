namespace Console.Networking.Packets.ConnectionResponse
{

    /// <summary>
    /// Gets sent if the Host has denied the Connection
    /// </summary>
    public class ConnectionRequestResponseFailedPacket : ConnectionRequestResponsePacket
    {

        /// <summary>
        /// Does the Host Allow the Connection?
        /// </summary>
        public override bool Success => false;

        /// <summary>
        /// Unique Identifier for this Packet
        /// </summary>
        public override string PacketIdentifier => typeof(ConnectionRequestResponsePacket).AssemblyQualifiedName;
        /// <summary>
        /// The Reason why the Connection Request is Denied.
        /// </summary>
        public readonly string Reason;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="reason">The Fail Reason</param>
        public ConnectionRequestResponseFailedPacket(string reason = null)
        {
            Reason = reason ?? "";
        }
    }
}