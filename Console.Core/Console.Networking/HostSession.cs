using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Console.Core;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;
using Console.Networking.Packets.ConnectionAbort;
using Console.Networking.Packets.Log;

namespace Console.Networking
{
    /// <summary>
    /// Host Session managing the Connected Clients.
    /// </summary>
    public class HostSession
    {
        /// <summary>
        /// Packet Receive gets invoked when any packet was received from any client
        /// </summary>
        /// <param name="socket">The Sender Socket</param>
        /// <param name="packet">The Transmitted Packet</param>
        public delegate void PacketReceive(ConsoleSocket socket, ANetworkPacket packet);

        /// <summary>
        /// Gets Invoked when a client connected to the host
        /// </summary>
        /// <param name="socket">The client that connected.</param>
        public delegate void ClientConnected(ConsoleSocket socket);

        /// <summary>
        /// Gets Invoked when a client disconnected from the host
        /// </summary>
        /// <param name="socket">The client that disconnected.</param>
        public delegate void ClientDisconnecting(ConsoleSocket socket);

        /// <summary>
        /// OnPacketReceive Event. Invoked when a Packet gets Received.
        /// </summary>
        public static event PacketReceive OnPacketReceive;
        /// <summary>
        /// OnClientConnected Event. Invoked when a client disconnected
        /// </summary>
        public static event ClientConnected OnClientConnected;

        /// <summary>
        /// The TCP Listener used to detect connection attempts.
        /// </summary>
        private static TcpListener Listener;

        /// <summary>
        /// The Listener Loop Thread.
        /// </summary>
        private static Thread LoopThread;
        /// <summary>
        /// List of Connected Clients
        /// </summary>
        private static readonly List<ConsoleSocket> Clients = new List<ConsoleSocket>();
        /// <summary>
        /// Flag that gets set to true when the Host is running.
        /// </summary>
        public static bool IsRunning { get; private set; }

        /// <summary>
        /// Broadcasts all Logs received to every client that is connected.
        /// </summary>
        /// <param name="log">The log that got received.</param>
        private void OnLog(string log)
        {
            Clients.Where(x => x.IsAuthenticated).ToList().ForEach(x => x.TrySendPacket(new LogPacket(log)));
        }

        /// <summary>
        /// Removes a Client from the Connected Clients List
        /// </summary>
        /// <param name="socket">Socket to Remove</param>
        public void RemoveClient(ConsoleSocket socket)
        {
            socket.Dispose();
            Clients.Remove(socket);
        }

        /// <summary>
        /// Gracefully Disconnects the Client
        /// </summary>
        /// <param name="socket">Socket to Disconnect.</param>
        public void DisconnectClient(ConsoleSocket socket)
        {
            if (!socket.Connected || socket.IsDisposed)
            {
                return;
            }
            socket.TrySendPacket(new ConnectionAbortPacket("Host Closed Connection"));
            socket.Dispose();
        }


        /// <summary>
        /// Disconnects All Clients Gracefully.
        /// </summary>
        public void DisconnectClients()
        {
            Clients.Where(x => x != null && x.Connected && !x.IsDisposed).ToList().ForEach(DisconnectClient);
            Clients.Clear();
        }

        /// <summary>
        /// Registers an IPacketHostHandler to the OnPacketReceive Event.
        /// </summary>
        /// <param name="handler">IPacketHostHandler implementation</param>
        public void RegisterHandler(IPacketHostHandler handler)
        {
            RegisterHandler(handler._Handle);
        }

        /// <summary>
        /// Registers an IPacketHostHandler to the OnPacketReceive Event.
        /// </summary>
        /// <param name="handler">PacketReceive Handler</param>
        public void RegisterHandler(PacketReceive handler)
        {
            OnPacketReceive += handler;
        }

        /// <summary>
        /// Starts the Host Process on the Specified Port.
        /// </summary>
        /// <param name="port"></param>
        public void StartHost(int port)
        {
            NetworkedInitializer.Logger.Log("Starting Console Host on port: " + port);
            AConsoleManager.OnLog += OnLog;
            Listener?.Stop();
            Listener = TcpListener.Create(port);
            Listener.Start();

            IsRunning = true;
            LoopThread = new Thread(ListenerLoop);
            LoopThread.Start();
            NetworkedInitializer.Logger.Log("Host Initialized.");
        }

        /// <summary>
        /// Stops the Host Process.
        /// </summary>
        public void StopHost()
        {
            AConsoleManager.OnLog -= OnLog;
            NetworkedInitializer.Logger.Log("Stopping Console Host");
            IsRunning = false;
            Listener?.Stop();
        }

        /// <summary>
        /// Forces the Host Process to Abort.
        /// </summary>
        public void ForceStopHost()
        {
            AConsoleManager.OnLog -= OnLog;
            NetworkedInitializer.Logger.LogWarning("Force Stopping Console Host");
            IsRunning = false;
            Listener?.Stop();
            LoopThread?.Abort();

            DisconnectClients();
        }

        /// <summary>
        /// Removes all Inactive Clients from the Host.
        /// </summary>
        private void RemoveInactiveClients()
        {
            for (int i = Clients.Count - 1; i >= 0; i--)
            {
                if (Clients[i].IsDisposed || !Clients[i].Connected)
                {

                    Clients.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// The Listener Loop Thread Method.
        /// </summary>
        private void ListenerLoop()
        {
            while (IsRunning)
            {
                if (Listener.Pending())
                {
                    ConsoleSocket cs = new ConsoleSocket(Listener.AcceptTcpClient());
                    NetworkedInitializer.Logger.Log($"Client {cs} connected.");

                    if (NetworkingSettings.AllowConnections)
                    {
                        cs.OnPacketReceive += package => OnPacketReceive?.Invoke(cs, package);
                        Clients.Add(cs);
                        OnClientConnected?.Invoke(cs);
                    }
                    else
                    {
                        NetworkedInitializer.Logger.LogWarning($"Client {cs} closed. (Connection not Allowed)");
                        cs.TrySendPacket(new ConnectionAbortPacket("Host does not allow Connections at this point."));
                        cs.Dispose();
                    }
                }

                if (Clients.Count == 0)
                {
                    continue;
                }

                Clients.ForEach(x => x.ProcessPacket());
                RemoveInactiveClients();
            }
            DisconnectClients();
        }
    }
}