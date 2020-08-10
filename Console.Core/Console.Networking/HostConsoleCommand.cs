using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Console.Core.Attributes.CommandSystem;
using Console.Core.Console;
using Console.Core.PropertySystem;

namespace Console.Networking
{
    public class HostConsoleCommand
    {
        public static readonly ConcurrentQueue<string> Commands = new ConcurrentQueue<string>();
        public static readonly ConcurrentQueue<string> LogQueue = new ConcurrentQueue<string>();
        private static TcpListener Listener;
        private static Thread LoopThread;
        private static readonly List<TcpClient> Clients = new List<TcpClient>();
        public static bool IsRunning { get; private set; }

        [Property("networking.host.allowconnections")]
        private static bool AllowConnections = true;

        private void OnLog(string log)
        {
            LogQueue.Enqueue(log);
        }

        [Command("start-console-host", "Creates a Console host at the specified port.", "start-host")]
        private void StartHostCommand(int port)
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


        [Command("stop-console-host", "Stops the Console Host Server", "stop-host")]
        private void StopHostCommand()
        {
            AConsoleManager.OnLog -= OnLog;
            AConsoleManager.Instance.Log("Stopping Console Host");
            IsRunning = false;
            Listener?.Stop();
        }

        [Command("abort-console-host", "Aborts the Console Host Server", "abort-host")]
        private void ForceStopHostCommand()
        {
            AConsoleManager.OnLog -= OnLog;
            AConsoleManager.Instance.LogWarning("Force Stopping Console Host");
            IsRunning = false;
            Listener?.Stop();
            LoopThread?.Abort();
        }

        public static void ProcessQueue()
        {
            if (!IsRunning) return;
            string command;
            while (!Commands.IsEmpty && Commands.TryDequeue(out command))
            {
                AConsoleManager.Instance.Log("Running Command on Host: " + command);
                AConsoleManager.Instance.EnterCommand(command);
            }
        }

        private static void ListenerLoop()
        {
            while (IsRunning)
            {
                if (Listener.Pending())
                {
                    TcpClient cl = Listener.AcceptTcpClient();
                    AConsoleManager.Instance.Log($"Client {cl} connected.");
                    if (AllowConnections)
                    {
                        Clients.Add(cl);
                    }
                    else
                    {
                        AConsoleManager.Instance.LogWarning($"Client {cl} closed. (Connection not Allowed)");
                        cl.Close();
                    }
                }

                if (Clients.Count == 0) continue;


                string logs = "";
                while (!LogQueue.IsEmpty && LogQueue.TryDequeue(out string l))
                {
                    logs += l;
                }
                byte[] logData = Encoding.ASCII.GetBytes(logs + '\0');

                for (int i = 0; i < Clients.Count; i++)
                {
                    if (Clients[i].Available != 0)
                    {
                        byte[] data = new byte[Clients[i].Available];
                        Clients[i].GetStream().Read(data, 0, data.Length);

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
                        commands.ForEach(Commands.Enqueue);


                    }

                    if (logData.Length > 1) // '\0'
                    {
                        Clients[i].GetStream().Write(logData, 0, logData.Length);
                    }
                }
            }
        }
    }
}