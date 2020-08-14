using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using Console.Core.Console;
using Console.Networking.Handlers;
using Console.Networking.Handlers.Abstract;
using Console.Networking.Packets;
using Console.Networking.Packets.Command;
using Console.Networking.Packets.ConnectionAbort;
using Console.Networking.Packets.ConnectionRequest;

namespace Console.Networking
{
    public class ClientSession
    {
        public enum ConnectionState { Idle, Connecting, Connected, Error }
        public ConsoleSocket Client { get; } = new ConsoleSocket();
        private Thread ConnectThread;
        public ConnectionState State;


        public void RegisterHandler(IPacketClientHandler handler)
        {
            Client.OnPacketReceive += handler._Handle;
        }

        public void Connect(string ip, int port)
        {
            if (State == ConnectionState.Connecting) return;
            State = ConnectionState.Connecting;
            AConsoleManager.Instance.Log("Connecting...");
            ConnectThread = new Thread(() => ConnectionThread(ip, port));
            ConnectThread.Start();
        }
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
                AConsoleManager.Instance.LogWarning("Error Connecting: " + e);
                State = ConnectionState.Error;
            }
        }


        public void Disconnect()
        {
            Client.TrySendPacket(new ConnectionAbortPacket("Client Disconnected"));
            Client?.Dispose();
            AConsoleManager.Instance.Log("Disconnected from Host");
        }

        public void RunCommand(string cmd)
        {
            if (Client == null || !Client.IsAuthenticated || !Client.Connected || !Client.TrySendPacket(new CommandPacket(true, cmd)))
            {
                AConsoleManager.Instance.LogWarning("Can not run command. No connection to host.\nRun connect-console command to connect to a host");
            }
        }

        public void ProcessLogMessages()
        {

            if (State == ConnectionState.Error)
            {
                AConsoleManager.Instance.LogWarning("Can not connect..");
                State = ConnectionState.Idle;
            }
            if (State == ConnectionState.Connected)
            {
                AConsoleManager.Instance.Log("Connected.");
                State = ConnectionState.Idle;
            }


            if (Client == null || !Client.Connected)
                return;
            for (int i = 0; i < NetworkingSettings.ClientPacketsPerTick; i++)
            {
                Client?.ProcessPacket();
            }


        }
    }
}