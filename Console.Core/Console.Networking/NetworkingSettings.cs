using System;
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
        /// Mutes all logs from the networking system.
        /// </summary>
        [Property("logs.networking.mute")]
        private static bool MuteLogs
        {
            get => NetworkedInitializer.Logger.Mute;
            set => NetworkedInitializer.Logger.Mute = value;
        }

        /// <summary>
        /// Does not Send Logs from the networking layer if true.
        /// Has no effect when MuteLogs is true
        /// </summary>
        [Property("logs.networking.layer.mute")]
        public static bool MuteLayerLogs;

        /// <summary>
        /// Encoding that is used for communication
        /// </summary>
        [Property("networking.text.encoding")]
        public static string Encoding = "ASCII";

        /// <summary>
        /// If True the Host will allow the SendFileRequest from a client
        /// </summary>
        [Property("networking.send.allowreceive")]
        public static bool AllowReceive = true;

        /// <summary>
        /// If True the Client is able to Send Files to the host.
        /// </summary>
        [Property("networking.send.allowsend")]
        public static bool AllowSend = true;

        [Property("networking.wget.enabled")]
        public static bool WGetAllow = true;
        [Property("networking.runurl.enabled")]
        public static bool RunUrlAllow = true;

        /// <summary>
        /// The Encoding Instance.
        /// </summary>
        public static Encoding EncodingInstance => System.Text.Encoding.GetEncoding(Encoding);

        /// <summary>
        /// The Maximum Data Size of a single Packet.
        /// </summary>
        [Property("networking.packets.data.maxsize.bytes")]
        public static int PacketDataMaxBytes = 1024 * 4;
        /// <summary>
        /// The Maximum ID Size of a single Packet.
        /// </summary>
        [Property("networking.packets.header.maxsize.bytes")]
        public static int PacketIdentifierMaxBytes = 1024;

        /// <summary>
        /// The Maximum Data Size of a single Packet.
        /// </summary>
        [Property("networking.packets.data.maxsize.kb")]
        public static int PacketDataMaxSizeKB
        {
            get => PacketDataMaxBytes / 1024;
            set => PacketDataMaxBytes = value * 1024;
        }
        /// <summary>
        /// The Maximum Data Size of a single Packet.
        /// </summary>
        [Property("networking.packets.data.maxsize.mb")]
        public static int PacketDataMaxSizeMB
        {
            get => PacketDataMaxSizeKB / 1024;
            set => PacketDataMaxSizeKB = value * 1024;
        }

        /// <summary>
        /// The Maximum ID Size of a single Packet.
        /// </summary>
        [Property("networking.packets.header.maxsize.kb")]
        public static int PacketIdentifierMaxSizeKB
        {
            get => PacketIdentifierMaxBytes / 1024;
            set => PacketIdentifierMaxBytes = value * 1024;
        }



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
                if (string.Equals(value, _authenticator, StringComparison.InvariantCulture))
                {
                    return;
                }
                _authenticator = value;
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
        public static IAuthenticator AuthenticatorInstance {
            get
            {
                if (authenticatorInstance == null)
                {
                    authenticatorInstance = (IAuthenticator)Activator.CreateInstance(Type.GetType(_authenticator));

                }

                return authenticatorInstance;
            }
        }


        /// <summary>
        /// The IAuthenticator Instance Backing Field.
        /// </summary>
        private static IAuthenticator authenticatorInstance;
    }
}