using Console.Networking.Packets.Abstract;

/// <summary>
/// ConnectionRequestResponsePacket Classes
/// </summary>
namespace Console.Networking.Packets.ConnectionResponse
{
    /// <summary>
    /// Packet that gets Returned by the Host.
    /// </summary>
    public abstract class ConnectionRequestResponsePacket : ANetworkPacket
    {
        /// <summary>
        /// Does the Host Allow the Connection?
        /// </summary>
        public virtual bool Success { get; }
    }
}