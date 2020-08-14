using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Log
{
    /// <summary>
    /// Gets sent from the host to the client.
    /// </summary>
    public class LogPacket:ANetworkPacket
    {
        /// <summary>
        /// Log Line written on the Host.
        /// </summary>
        public readonly string LogLine;

        public LogPacket(string logLine)
        {
            LogLine = logLine;
        }
    }
}