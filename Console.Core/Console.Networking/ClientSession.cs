using System;
using System.Threading;

using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Command;
using Console.Networking.Packets.ConnectionAbort;
using Console.Networking.Packets.ConnectionRequest;

namespace Console.Networking
{
    /// <summary>
    /// Client Session Used to Connect to a Host Session
    /// </summary>
    public class ClientSession
    {

        /// <summary>
        /// The Connection State of the Client Session
        /// </summary>
        public enum ConnectionState
        {

            /// <summary>
            /// The Client Session is Ready to Connect
            /// </summary>
            Idle,

            /// <summary>
            /// The Client Session is trying to connect to a Host.
            /// </summary>
            Connecting,

            /// <summary>
            /// The Client Session Successfully Connected to the Host
            /// </summary>
            Connected,

            /// <summary>
            /// The Connection Failed.
            /// </summary>
            Error

        }

        /// <summary>
        /// The Connect Thread used to implement non blocking Connect.
        /// </summary>
        private Thread ConnectThread;

        /// <summary>
        /// The Current Client Session State.
        /// </summary>
        public ConnectionState State;

        /// <summary>
        /// The Socket used to Communicate with the Host.
        /// </summary>
        public ConsoleSocket Client { get; } = new ConsoleSocket();


        /// <summary>
        /// Registers an IPacketClientHandler to the OnPacketReceive Event.
        /// </summary>
        /// <param name="handler">Handler to add.</param>
        public void RegisterHandler(IPacketClientHandler handler)
        {
            Client.OnPacketReceive += handler._Handle;
        }

        /// <summary>
        /// Connects to the Specified Host
        /// </summary>
        /// <param name="ip">Host IP</param>
        /// <param name="port">Host Port</param>
        public void Connect(string ip, int port)
        {
            if (State == ConnectionState.Connecting)
            {
                return;
            }

            State = ConnectionState.Connecting;
            NetworkedInitializer.Logger.Log("Connecting...");
            ConnectThread = new Thread(() => ConnectionThread(ip, port));
            ConnectThread.Start();
        }

        /// <summary>
        /// Thread Function. Used to Implement NonBlocking Connection
        /// </summary>
        /// <param name="ip">Host IP</param>
        /// <param name="port">Host Port</param>
        private void ConnectionThread(string ip, int port)
        {
            try
            {
                Client.Connect(ip, port);
                Client.TrySendPacket(new ConnectionRequestPacket(NetworkingSettings.NetworkVersion.ToString()));
                State = ConnectionState.Connected;
            }
            catch (Exception e)
            {
                NetworkedInitializer.Logger.LogWarning("Error Connecting: " + e);
                State = ConnectionState.Error;
            }
        }


        /// <summary>
        /// Gracefully Disconnects the Client from the Host.
        /// </summary>
        public void Disconnect()
        {
            Client?.TrySendPacket(new ConnectionAbortPacket("Client Disconnected"));
            Client?.Dispose();
            NetworkedInitializer.Logger.Log("Disconnected from Host");
        }

        /// <summary>
        /// Runs a Command on the Host Machine.
        /// </summary>
        /// <param name="cmd"></param>
        public void RunCommand(string cmd)
        {
            if (Client == null ||
                !Client.IsAuthenticated ||
                !Client.Connected ||
                !Client.TrySendPacket(new CommandPacket(true, cmd)))
            {
                NetworkedInitializer.Logger.LogWarning(
                                                       "Can not run command. No connection to host.\nRun connect-console command to connect to a host"
                                                      );
            }
        }

        /// <summary>
        /// Gets Invoked by the NetworkedConsoleProcess Class every ConsoleTick
        /// </summary>
        public void ProcessLogMessages()
        {
            if (State == ConnectionState.Error)
            {
                NetworkedInitializer.Logger.LogWarning("Can not connect..");
                State = ConnectionState.Idle;
            }

            if (State == ConnectionState.Connected)
            {
                NetworkedInitializer.Logger.Log("Connected.");
                State = ConnectionState.Idle;
            }


            if (Client == null || !Client.Connected)
            {
                return;
            }

            for (int i = 0; i < NetworkingSettings.ClientPacketsPerTick; i++)
            {
                Client?.ProcessPacket();
            }
        }

    }
}