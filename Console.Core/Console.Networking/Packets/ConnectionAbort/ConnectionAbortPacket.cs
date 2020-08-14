using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.ConnectionAbort
{
    /// <summary>
    /// Gets sent from the host or client to communicate the end of the connection
    /// </summary>
    public class ConnectionAbortPacket : ANetworkPacket
    {
        private readonly string _reason;
        /// <summary>
        /// True if the Connection Abort has any Reason Attached.
        /// </summary>
        public readonly bool HasReason;
        /// <summary>
        /// The Reason of the Connection Abort
        /// </summary>
        public string Reason => _reason ?? "No Reason";
        public override bool DoNotEncrypt => true;

        public ConnectionAbortPacket(string reason=null)
        {
            _reason = reason;
            HasReason = _reason != null;
        }
    }
}