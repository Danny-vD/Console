using Console.Networking.Packets.Abstract;


/// <summary>
/// ConnectionAbortPacket Classes
/// </summary>
namespace Console.Networking.Packets.ConnectionAbort
{
    /// <summary>
    /// Gets sent from the host or client to communicate the end of the connection
    /// </summary>
    public class ConnectionAbortPacket : ANetworkPacket
    {
        /// <summary>
        /// Reason Backing Field
        /// </summary>
        private readonly string _reason;
        /// <summary>
        /// True if the Connection Abort has any Reason Attached.
        /// </summary>
        public readonly bool HasReason;
        /// <summary>
        /// The Reason of the Connection Abort
        /// </summary>
        public string Reason => _reason ?? "No Reason";

        /// <summary>
        /// Flag that specifies that the networking layer should not encrypt/decrypt the packet.
        /// </summary>
        public override bool DoNotEncrypt => true;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="reason">The Connection Abort Reason</param>
        public ConnectionAbortPacket(string reason = null)
        {
            _reason = reason;
            HasReason = _reason != null;
        }
    }
}