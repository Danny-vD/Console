using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console.Attributes.CommandSystem;
using Console.Console;

namespace Console.NetworkCommands
{
    public class ClientConsoleCommand
    {
        private enum ConnectionState { Idle, Connecting, Connected, Error }
        private static TcpClient Client;
        private static Thread ConnectThread;
        private static ConnectionState State;

        [Command("connect-console", "Tries to connect to a hosting console.", "connect")]
        private void ConnectConsoleCommand(string ip, int port)
        {
            if (State == ConnectionState.Connecting) return;
            Client?.Dispose();
            Client = new TcpClient();
            State = ConnectionState.Connecting;
            ConsoleManager.Log("Connecting...");
            Thread t = new Thread(() => ConnectionThread(ip, port));
            t.Start();
        }

        private void ConnectionThread(string ip, int port)
        {
            try
            {
                Client.Connect(ip, port);
                State = ConnectionState.Connected;
            }
            catch (Exception e)
            {
                State = ConnectionState.Error;
                return;
            }


            ConsoleManager.Log("Connection Established.");
        }

        [Command("disconnect-console", "Tries to disconnect from a hosting console", "disconnect")]
        private void DisconnectConsoleCommand()
        {
            Client?.Dispose();
            ConsoleManager.Log("Disconnected from Host");
        }

        [Command("hrun", "Runs a Remote Command on the connected host.")]
        private void RunHostCommand(string command)
        {
            if (Client?.Client != null && Client.Connected)
            {
                string cmd = command + '\0';
                byte[] data = Encoding.ASCII.GetBytes(cmd);
                Client.GetStream().Write(data, 0, data.Length);
            }
            else
            {
                ConsoleManager.LogWarning("Can not run command. No connection to host.\nRun connect-console command to connect to a host");
            }
        }

        public static void ProcessLogMessages()
        {

            if (State == ConnectionState.Error)
            {
                ConsoleManager.LogWarning("Can not connect..");
                State = ConnectionState.Idle;
            }
            if (State == ConnectionState.Connected)
            {
                ConsoleManager.Log("Connected.");
                State = ConnectionState.Idle;
            }


            if (Client?.Client == null || !Client.Connected) return;

            if (Client.Available != 0)
            {
                byte[] data = new byte[Client.Available];
                Client.GetStream().Read(data, 0, data.Length);

                List<byte[]> datas = new List<byte[]>();
                int last = 0;
                for (int j = last; j < data.Length; j++)
                {
                    if (data[j] == (byte)'\0')
                    {

                        byte[] d = new byte[j - last];
                        for (int k = last; k < j; k++)
                        {
                            d[k - last] = data[k];
                        }

                        datas.Add(d);
                        last = j + 1;
                    }
                }

                List<string> commands = datas.Select(x => Encoding.ASCII.GetString(x)).ToList();
                commands.ForEach(ConsoleManager.LogPlainText);

            }
        }

    }
}