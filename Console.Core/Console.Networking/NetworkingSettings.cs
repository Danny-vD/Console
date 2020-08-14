using System;
using System.Reflection;
using System.Text;
using Console.Core.Console;
using Console.Core.PropertySystem;
using Console.Networking.Authentication;
using Console.Networking.Handlers;

namespace Console.Networking
{
    public class NetworkingSettings
    {

        [Property("networking.text.encoding")]
        public static string Encoding = "ASCII";
        public static Encoding EncodingInstance => System.Text.Encoding.GetEncoding(Encoding);

        [Property("networking.packets.maxdatasize")]
        public static int PacketDataMaxSize = 1024 * 4;
        [Property("networking.packets.maxidsize")]
        public static int PacketIdentifierMaxSize = 1024;

        [Property("version.networking")]
        public static Version NetworkVersion => Assembly.GetExecutingAssembly().GetName().Version;

        [Property("networking.host.allowconnections")]
        public static bool AllowConnections = true;

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
        private static string _authenticator = typeof(DefaultAuthenticator).AssemblyQualifiedName;

        [Property("networking.client.packetspertick")]
        public static int ClientPacketsPerTick = 5;

        [Property("networking.socket.packetwaitms")]
        public static int PacketWaitSleepTimer = 1000;

        public static ClientSession ClientSession = new ClientSession();
        public static HostSession HostSession = new HostSession();
        public static IAuthenticator AuthenticatorInstance { get; private set; }
    }
}