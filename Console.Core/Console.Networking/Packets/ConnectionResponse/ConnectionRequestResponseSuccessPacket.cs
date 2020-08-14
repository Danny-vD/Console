namespace Console.Networking.Packets.ConnectionResponse
{
    /// <summary>
    /// Gets sent after the Host has accepted the connection to the client.
    /// </summary>
    public class ConnectionRequestResponseSuccessPacket : ConnectionRequestResponsePacket
    {
        public override bool DoNotEncrypt => true;
        public override bool Success => true;
        public override string PacketIdentifier => typeof(ConnectionRequestResponsePacket).AssemblyQualifiedName;

        /// <summary>
        /// The Authentication Method of the host.
        /// </summary>
        public readonly string AuthMethod;

        public ConnectionRequestResponseSuccessPacket(string authMethod)
        {
            AuthMethod = authMethod;
        }
    }
}