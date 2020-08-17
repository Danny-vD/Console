using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.Log;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the LogPacket when sent from the Host.
    /// </summary>
    public class LogClientHandler : APacketClientHandler<LogPacket>
    {
        /// <summary>
        /// Handles the Packet
        /// </summary>
        /// <param name="item">The Packet</param>
        public override void Handle(LogPacket item)
        {
            AConsoleManager.Instance.LogPlainText(item.LogLine);
        }
    }
}