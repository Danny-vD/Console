using System.Reflection;
using Console.Core;
using Console.Core.ActivationSystem;
using Console.Core.ExtensionSystem;
using Console.Core.PropertySystem;
using Console.Networking.Authentication;
using Console.Networking.Commands;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;

namespace Console.Networking
{
    public class NetworkedInitializer : AExtensionInitializer
    {
        public static NetworkedConsoleProcess Instance { get; private set; }
        public override void Initialize()
        {
            IPacketClientHandler[] ch = ActivateOnAttributeUtils.ActivateObjects<IPacketClientHandler>(Assembly.GetExecutingAssembly());
            IPacketHostHandler[] hh = ActivateOnAttributeUtils.ActivateObjects<IPacketHostHandler>(Assembly.GetExecutingAssembly());
            IPacketSerializer[] ps =
                ActivateOnAttributeUtils.ActivateObjects<IPacketSerializer>(Assembly.GetExecutingAssembly());
            foreach (IPacketSerializer packetSerializer in ps)
            {
                SerializerCollection.AddSerializer(packetSerializer);
            }

            foreach (IPacketClientHandler packetClientHandler in ch)
            {
                NetworkingSettings.ClientSession.RegisterHandler(packetClientHandler);
            }

            foreach (IPacketHostHandler packetHostHandler in hh)
            {
                NetworkingSettings.HostSession.RegisterHandler(packetHostHandler);
            }


            PropertyAttributeUtils.AddProperties(typeof(Cryptography));
            PropertyAttributeUtils.AddProperties<NetworkingSettings>();
            PropertyAttributeUtils.AddProperties<HostConsoleCommand>();
            PropertyAttributeUtils.AddProperties<ClientConsoleCommand>();
            PropertyAttributeUtils.AddProperties<SymmetricBlockAuthenticator>();
            Instance = new NetworkedConsoleProcess();
            //NetworkingSettings.ClientSession.RegisterHandler(new LogClientHandler());
            //NetworkingSettings.ClientSession.RegisterHandler(new ConnectionRequestResponseClientHandler());
            //NetworkingSettings.ClientSession.RegisterHandler(new ConnectionAbortPacketClientHandler());
            //NetworkingSettings.ClientSession.RegisterHandler(new AuthenticationPacketClientHandler());
            //NetworkingSettings.ClientSession.RegisterHandler(new AuthenticationResultPacketHandler());
            //NetworkingSettings.HostSession.RegisterHandler(new CommandHostHandler());
            //NetworkingSettings.HostSession.RegisterHandler(new ConnectionRequestHostHandler());
            //NetworkingSettings.HostSession.RegisterHandler(new ConnectionAbortPacketHostHandler());
            //NetworkingSettings.HostSession.RegisterHandler(new ConnectionAuthRequestPacketHostHandler());
            //SerializerCollection.AddSerializer(new CommandPacketSerializer());
            //SerializerCollection.AddSerializer(new ConnectionRequestSerializer());
            //SerializerCollection.AddSerializer(new ConnectionRequestResponseSerializer());
            //SerializerCollection.AddSerializer(new LogPacketSerializer());
            //SerializerCollection.AddSerializer(new AuthenticationPacketSerializer());
            //SerializerCollection.AddSerializer(new AuthenticationResultPacketSerializer());
            //SerializerCollection.AddSerializer(new AuthenticationRequestPacketSerializer());

            AConsoleManager.Instance.Log("Default Salt: " + Cryptography.DefaultSalt);
            AConsoleManager.Instance.Log("Default Vector: " + Cryptography.DefaultVector);

        }
    }
}