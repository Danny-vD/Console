using Console.Core.Console;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Command;

namespace Console.Networking.Handlers
{
    public class CommandHostHandler : APacketHostHandler<CommandPacket>
    {
        public override void Handle(ConsoleSocket client, CommandPacket item)
        {
            AConsoleManager.Instance.Log("Running Command on Host: " + item.Input);
            AConsoleManager.Instance.EnterCommand(item.Input);
        }
    }
}