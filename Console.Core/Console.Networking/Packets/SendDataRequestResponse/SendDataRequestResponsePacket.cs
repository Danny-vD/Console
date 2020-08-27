using Console.Networking.Packets.Abstract;

/// <summary>
/// SendDataRequestResponsePacket Classes
/// </summary>
namespace Console.Networking.Packets.SendDataRequestResponse
{
    /// <summary>
    /// The Packet that gets returned by the Host that specifies if the File Is allowed to be sent
    /// </summary>
    public class SendDataRequestResponsePacket : ANetworkPacket
    {
        /// <summary>
        /// Flag that Determines if the File is allowed.
        /// </summary>
        public readonly bool Allowed;

        /// <summary>
        /// Private Constructor
        /// </summary>
        /// <param name="allow">Allow the File Transfer?</param>
        private SendDataRequestResponsePacket(bool allow)
        {
            Allowed = allow;
        }

        /// <summary>
        /// Allow Packet Response
        /// </summary>
        public static readonly SendDataRequestResponsePacket Allow = new SendDataRequestResponsePacket(true);

        /// <summary>
        /// Deny Packet Response
        /// </summary>
        public static readonly SendDataRequestResponsePacket Deny = new SendDataRequestResponsePacket(true);
    }
}