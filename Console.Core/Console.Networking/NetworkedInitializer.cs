using System;
using System.Reflection;
using Console.Core;
using Console.Core.Console;
using Console.Core.PropertySystem;
using Console.Core.Utils;
using Console.Networking.Authentication;
using Console.Networking.Commands;
using Console.Networking.Handlers;
using Console.Networking.Packets;
using Console.Networking.Packets.Authentication;
using Console.Networking.Packets.AuthenticationRequest;
using Console.Networking.Packets.AuthenticationResult;
using Console.Networking.Packets.Command;
using Console.Networking.Packets.ConnectionRequest;
using Console.Networking.Packets.ConnectionResponse;
using Console.Networking.Packets.Log;

namespace Console.Networking
{
    public class NetworkedInitializer : AExtensionInitializer
    {
        public static NetworkedConsoleProcess Instance { get; private set; }
        public override void Initialize()
        {
            PropertyAttributeUtils.AddProperties(typeof(Cryptography));
            PropertyAttributeUtils.AddProperties<NetworkingSettings>();
            PropertyAttributeUtils.AddProperties<HostConsoleCommand>();
            PropertyAttributeUtils.AddProperties<ClientConsoleCommand>();
            PropertyAttributeUtils.AddProperties<SymmetricBlockAuthenticator>();
            Instance = new NetworkedConsoleProcess();
            NetworkingSettings.ClientSession.RegisterHandler(new LogClientHandler());
            NetworkingSettings.ClientSession.RegisterHandler(new ConnectionRequestResponseClientHandler());
            NetworkingSettings.ClientSession.RegisterHandler(new ConnectionAbortPacketClientHandler());
            NetworkingSettings.ClientSession.RegisterHandler(new AuthenticationPacketClientHandler());
            NetworkingSettings.ClientSession.RegisterHandler(new AuthenticationResultPacketHandler());
            NetworkingSettings.HostSession.RegisterHandler(new CommandHostHandler());
            NetworkingSettings.HostSession.RegisterHandler(new ConnectionRequestHostHandler());
            NetworkingSettings.HostSession.RegisterHandler(new ConnectionAbortPacketHostHandler());
            NetworkingSettings.HostSession.RegisterHandler(new ConnectionAuthRequestPacketHostHandler());
            SerializerCollection.AddSerializer(new CommandPacketSerializer());
            SerializerCollection.AddSerializer(new ConnectionRequestSerializer());
            SerializerCollection.AddSerializer(new ConnectionRequestResponseSerializer());
            SerializerCollection.AddSerializer(new LogPacketSerializer());
            SerializerCollection.AddSerializer(new AuthenticationPacketSerializer());
            SerializerCollection.AddSerializer(new AuthenticationResultPacketSerializer());
            SerializerCollection.AddSerializer(new AuthenticationRequestPacketSerializer());

            AConsoleManager.Instance.Log("Default Salt: " + Cryptography.DefaultSalt);
            AConsoleManager.Instance.Log("Default Vector: " + Cryptography.DefaultVector);

        }
    }
}