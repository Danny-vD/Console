using Console.Networking.Packets.Abstract;

/// <summary>
/// SendDataRequestPacket Classes
/// </summary>
namespace Console.Networking.Packets.SendDataRequest
{
    /// <summary>
    /// A Packet that gets sent by the Client.
    /// </summary>
    public class SendDataRequestPacket : ANetworkPacket
    {

        /// <summary>
        /// The Destination file on the Host Machine
        /// </summary>
        public string Destination;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="destination">Destination File</param>
        public SendDataRequestPacket(string destination)
        {
            Destination = destination;
        }

    }
}