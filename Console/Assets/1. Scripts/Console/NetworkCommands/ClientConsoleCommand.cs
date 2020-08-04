using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Console.Attributes.CommandSystem;
using Console.Console;

namespace Console.NetworkCommands
{
    public class ClientConsoleCommand
    {
        private static TcpClient Client;

        [Command("connect-console", "Tries to connect to a hosting console.", "connect")]
        private void ConnectConsoleCommand(string ip, int port)
        {
            Client = new TcpClient(ip, port);
        }

        [Command("disconnect-console", "Tries to disconnect from a hosting console", "disconnect")]
        private void DisconnectConsoleCommand()
        {
            Client?.Dispose();
        }

        [Command("hrun", "Runs a Remote Command on the connected host.")]
        private void RunHostCommand(string command)
        {
            if (Client != null && Client.Connected)
            {
                string cmd = command + '\0';
                byte[] data = Encoding.ASCII.GetBytes(cmd);
                Client.GetStream().Write(data, 0, data.Length);
            }
        }

        public static void ProcessLogMessages()
        {
            if (Client == null || !Client.Connected) return;
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