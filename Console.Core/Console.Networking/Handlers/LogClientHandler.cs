using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets.Log;

namespace Console.Networking.Handlers
{
    public class LogClientHandler : APacketClientHandler<LogPacket>
    {
        public override void Handle(LogPacket item)
        {
            AConsoleManager.Instance.LogPlainText(item.LogLine);
        }
    }
}