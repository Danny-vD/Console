using Console.Networking.Packets.Abstract;

/// <summary>
/// LogPacket Classes
/// </summary>
namespace Console.Networking.Packets.Log
{
    /// <summary>
    /// Gets sent from the host to the client.
    /// </summary>
    public class LogPacket : ANetworkPacket
    {

        /// <summary>
        /// Log Line written on the Host.
        /// </summary>
        public readonly string LogLine;

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="logLine">Log Line to send.</param>
        public LogPacket(string logLine)
        {
            LogLine = logLine;
        }

    }
}