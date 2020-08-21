using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Command;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles the CommandPacket when sent from a Client
    /// </summary>
    public class CommandHostHandler : APacketHostHandler<CommandPacket>
    {
        /// <summary>
        /// Handles the packet of type T
        /// </summary>
        /// <param name="client">Sending Client</param>
        /// <param name="item">The Packet</param>
        public override void Handle(ConsoleSocket client, CommandPacket item)
        {
            NetworkedInitializer.Logger.Log("Running Command on Host: " + item.Input);
            AConsoleManager.Instance.EnterCommand(item.Input);
        }
    }
}