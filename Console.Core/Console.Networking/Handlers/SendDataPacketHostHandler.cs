using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.SendData;
using Console.Networking.Packets.SendDataRequest;
using Console.Networking.Packets.SendDataRequestResponse;
using Console.Networking.SendData;

namespace Console.Networking.Handlers
{
    /// <summary>
    /// Handles SendDataRequestPacket on the Host Side.
    /// </summary>
    public class SendDataRequestPacketHostHandler : APacketHostHandler<SendDataRequestPacket>
    {
        /// <summary>
        /// Handles the SendDataRequestPackets from clients
        /// </summary>
        /// <param name="client">Client Sender</param>
        /// <param name="item">Packet</param>
        public override void Handle(ConsoleSocket client, SendDataRequestPacket item)
        {
            if (NetworkingSettings.AllowReceive)
            {
                SendDataManager.OpenReceiveChannel(client, item.Destination);
                client.TrySendPacket(SendDataRequestResponsePacket.Allow);
            }
            else
            {
                client.TrySendPacket(SendDataRequestResponsePacket.Deny);
            }
        }
    }
}