namespace Console.Networking.Packets.ConnectionResponse
{
    /// <summary>
    /// Gets sent after the Host has accepted the connection to the client.
    /// </summary>
    public class ConnectionRequestResponseSuccessPacket : ConnectionRequestResponsePacket
    {

        /// <summary>
        /// The Authentication Method of the host.
        /// </summary>
        public readonly string AuthMethod;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="authMethod">The Hosts Authentication Method</param>
        public ConnectionRequestResponseSuccessPacket(string authMethod)
        {
            AuthMethod = authMethod;
        }

        /// <summary>
        /// Flag that specifies that the networking layer should not encrypt/decrypt the packet.
        /// </summary>
        public override bool DoNotEncrypt => true;

        /// <summary>
        /// Does the Host Allow the Connection?
        /// </summary>
        public override bool Success => true;

        /// <summary>
        /// Unique Identifier for this Packet
        /// </summary>
        public override string PacketIdentifier => typeof(ConnectionRequestResponsePacket).AssemblyQualifiedName;

    }
}