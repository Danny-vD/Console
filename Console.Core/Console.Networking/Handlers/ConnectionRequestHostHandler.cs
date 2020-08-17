﻿using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.ConnectionRequest;
using Console.Networking.Packets.ConnectionResponse;

namespace Console.Networking.Handlers
{
    public class ConnectionRequestHostHandler : APacketHostHandler<ConnectionRequestPacket>
    {

        public override void Handle(ConsoleSocket client, ConnectionRequestPacket item)
        {
            ConnectionRequestResponsePacket ret = null;
            if (item.Version != NetworkingSettings.NetworkVersion.ToString())
                ret = new ConnectionRequestResponseFailedPacket("Version Mismatch");
            else ret = new ConnectionRequestResponseSuccessPacket(NetworkingSettings.Authenticator);
            if (!client.TrySendPacket(ret))
            {
                AConsoleManager.Instance.LogWarning("Can not send Packet " + item);
            }
        }
    }
}