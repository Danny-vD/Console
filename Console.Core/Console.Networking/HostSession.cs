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
    public class HostSession
    {
        public delegate void PacketReceive(ConsoleSocket socket, ANetworkPacket packet);
        public delegate void ClientConnected(ConsoleSocket socket);

        public delegate void ClientDisconnecting(ConsoleSocket socket);

        public static event PacketReceive OnPacketReceive;
        public static event ClientConnected OnClientConnected;
        
        private static TcpListener Listener;
        private static Thread LoopThread;
        private static readonly List<ConsoleSocket> Clients = new List<ConsoleSocket>();
        public static bool IsRunning { get; private set; }

        private void OnLog(string log)
        {
            Clients.Where(x => x.IsAuthenticated).ToList().ForEach(x => x.TrySendPacket(new LogPacket(log)));
        }

        public void RemoveClient(ConsoleSocket socket)
        {
            socket.Dispose();
            Clients.Remove(socket);
        }

        public void DisconnectClient(ConsoleSocket socket)
        {
            if (!socket.Connected || socket.IsDisposed) return;
            socket.TrySendPacket(new ConnectionAbortPacket("Host Closed Connection"));
            socket.Dispose();
        }

        public void DisconnectClients()
        {
            Clients.Where(x => x != null && x.Connected && !x.IsDisposed).ToList().ForEach(DisconnectClient);
            Clients.Clear();
        }

        public void RegisterHandler(IPacketHostHandler handler)
        {
            OnPacketReceive += handler._Handle;
        }

        public void StartHost(int port)
        {
            AConsoleManager.Instance.Log("Starting Console Host on port: " + port);
            AConsoleManager.OnLog += OnLog;
            Listener?.Stop();
            Listener = TcpListener.Create(port);
            Listener.Start();

            IsRunning = true;
            LoopThread = new Thread(ListenerLoop);
            LoopThread.Start();
            AConsoleManager.Instance.Log("Host Initialized.");
        }

        public void StopHost()
        {
            AConsoleManager.OnLog -= OnLog;
            AConsoleManager.Instance.Log("Stopping Console Host");
            IsRunning = false;
            Listener?.Stop();
        }

        public void ForceStopHost()
        {
            AConsoleManager.OnLog -= OnLog;
            AConsoleManager.Instance.LogWarning("Force Stopping Console Host");
            IsRunning = false;
            Listener?.Stop();
            LoopThread?.Abort();

            DisconnectClients();
        }

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

        private void ListenerLoop()
        {
            while (IsRunning)
            {
                if (Listener.Pending())
                {
                    ConsoleSocket cs = new ConsoleSocket(Listener.AcceptTcpClient());
                    AConsoleManager.Instance.Log($"Client {cs} connected.");

                    if (NetworkingSettings.AllowConnections)
                    {
                        cs.OnPacketReceive += package => OnPacketReceive?.Invoke(cs, package);
                        Clients.Add(cs);
                        OnClientConnected?.Invoke(cs);
                    }
                    else
                    {
                        AConsoleManager.Instance.LogWarning($"Client {cs} closed. (Connection not Allowed)");
                        cs.TrySendPacket(new ConnectionAbortPacket("Host does not allow Connections at this point."));
                        cs.Dispose();
                    }
                }

                if (Clients.Count == 0) continue;

                Clients.ForEach(x => x.ProcessPacket());
                RemoveInactiveClients();
            }
            DisconnectClients();
        }
    }
}