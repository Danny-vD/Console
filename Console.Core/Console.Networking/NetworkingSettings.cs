﻿using System;
using System.Reflection;
using System.Text;
using Console.Core.PropertySystem;
using Console.Networking.Authentication;

namespace Console.Networking
{
    /// <summary>
    /// Global Networking Settings.
    /// </summary>
    public class NetworkingSettings
    {

        /// <summary>
        /// Encoding that is used for communication
        /// </summary>
        [Property("networking.text.encoding")]
        public static string Encoding = "ASCII";

        /// <summary>
        /// The Encoding Instance.
        /// </summary>
        public static Encoding EncodingInstance => System.Text.Encoding.GetEncoding(Encoding);

        /// <summary>
        /// The Maximum Data Size of a single Packet.
        /// </summary>
        [Property("networking.packets.maxdatasize")]
        public static int PacketDataMaxSize = 1024 * 4;
        /// <summary>
        /// The Maximum ID Size of a single Packet.
        /// </summary>
        [Property("networking.packets.maxidsize")]
        public static int PacketIdentifierMaxSize = 1024;



        /// <summary>
        /// Version of the Networking Extension
        /// </summary>
        [Property("version.networking")]
        public static Version NetworkVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// If set to false the Host will not accept any new connections.
        /// </summary>
        [Property("networking.host.allowconnections")]
        public static bool AllowConnections = true;

        /// <summary>
        /// The IAuthenticator Implementation that is used for authentication
        /// </summary>
        [Property("networking.auth.provider")]
        public static string Authenticator
        {
            get => _authenticator;
            set
            {
                if (value == _authenticator) return;
                _authenticator = value;
                AuthenticatorInstance = (IAuthenticator)Activator.CreateInstance(Type.GetType(_authenticator));
            }
        }
        /// <summary>
        /// Backing field with default Value.
        /// </summary>
        private static string _authenticator = typeof(DefaultAuthenticator).AssemblyQualifiedName;

        /// <summary>
        /// Specifies how many packets are allowed to be processed per tick
        /// </summary>
        [Property("networking.client.packetspertick")]
        public static int ClientPacketsPerTick = 5;

        /// <summary>
        /// Specifies the time that the Networking Layer will wait when the packet is still beeing transmitted.
        /// </summary>
        [Property("networking.socket.packetwaitms")]
        public static int PacketWaitSleepTimer = 1000;

        /// <summary>
        /// The Client Session for the Console
        /// </summary>
        public static ClientSession ClientSession = new ClientSession();

        /// <summary>
        /// The Host Session for the Console
        /// </summary>
        public static HostSession HostSession = new HostSession();

        /// <summary>
        /// The IAuthenticator Instance.
        /// </summary>
        public static IAuthenticator AuthenticatorInstance { get; private set; }
    }
}