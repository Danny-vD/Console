using System;
using System.Reflection;

using Console.Core.ActivationSystem;
using Console.Core.CommandSystem.Attributes;
using Console.Core.ExtensionSystem;
using Console.Core.LogSystem;
using Console.Core.PropertySystem;
using Console.Networking.Authentication;
using Console.Networking.Commands;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;


/// <summary>
/// The Networking Extension is used to Connect 2 Instances of the Console Library to allow running commands and receiving logs on the remote instance.
/// </summary>
namespace Console.Networking
{
    /// <summary>
    /// Initializer of the Networking Extension
    /// </summary>
    public class NetworkedInitializer : AExtensionInitializer
    {

        /// <summary>
        /// Logger for this Extension
        /// </summary>
        public static ALogger Logger => GetLogger(Assembly.GetExecutingAssembly());

        /// <summary>
        /// The Console Process Instance.
        /// </summary>
        public static NetworkedConsoleProcess Instance { get; private set; }

        /// <summary>
        /// Initialization Function
        /// </summary>
        protected override void Initialize()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
                                                   {
                                                       if (NetworkingSettings.IsClient)
                                                       {
                                                           NetworkingSettings.ClientSession.Disconnect();
                                                       }

                                                       if (NetworkingSettings.IsHost)
                                                       {
                                                           NetworkingSettings.HostSession.ForceStopHost();
                                                       }
                                                   };

            IPacketClientHandler[] ch =
                ActivateOnAttributeUtils.ActivateObjects<IPacketClientHandler>(Assembly.GetExecutingAssembly());
            IPacketHostHandler[] hh =
                ActivateOnAttributeUtils.ActivateObjects<IPacketHostHandler>(Assembly.GetExecutingAssembly());
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
            PropertyAttributeUtils.AddProperties(typeof(WebConsoleCommands));
            PropertyAttributeUtils.AddProperties<NetworkingSettings>();
            PropertyAttributeUtils.AddProperties<HostConsoleCommand>();
            PropertyAttributeUtils.AddProperties<ClientConsoleCommand>();
            PropertyAttributeUtils.AddProperties<SymmetricBlockAuthenticator>();
            Instance = new NetworkedConsoleProcess();
            CommandAttributeUtils.AddCommands(typeof(WebConsoleCommands));
        }

    }
}