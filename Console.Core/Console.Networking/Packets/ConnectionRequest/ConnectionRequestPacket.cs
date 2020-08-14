using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionRequest
{
    /// <summary>
    /// Packet that gets sent by the client to establish a connection between host and client
    /// This is the first package that gets sent.
    /// </summary>
    public class ConnectionRequestPacket:ANetworkPacket
    {
        public override bool DoNotEncrypt => true;
        /// <summary>
        /// The Clients Version 
        /// </summary>
        public readonly string Version;

        public ConnectionRequestPacket(string version)
        {
            Version = version;
        }
    }
}