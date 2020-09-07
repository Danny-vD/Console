using Console.Networking.Packets.Abstract;

/// <summary>
/// ConnectionRequestPacket Classes
/// </summary>
namespace Console.Networking.Packets.ConnectionRequest
{
    /// <summary>
    /// Packet that gets sent by the client to establish a connection between host and client
    /// This is the first package that gets sent.
    /// </summary>
    public class ConnectionRequestPacket : ANetworkPacket
    {

        /// <summary>
        /// The Clients Version 
        /// </summary>
        public readonly string Version;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="version">Console Core Version</param>
        public ConnectionRequestPacket(string version)
        {
            Version = version;
        }

        /// <summary>
        /// Flag that specifies that the networking layer should not encrypt/decrypt the packet.
        /// </summary>
        public override bool DoNotEncrypt => true;

    }
}