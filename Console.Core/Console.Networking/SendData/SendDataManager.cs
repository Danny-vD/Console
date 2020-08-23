using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Console.Networking.Packets;
using Console.Networking.Packets.Abstract;
using Console.Networking.Packets.SendData;
using Console.Networking.Packets.SendDataRequest;
using Console.Networking.Packets.SendDataRequestResponse;

namespace Console.Networking.SendData
{

    /// <summary>
    /// Manages the SendDataPacket Response Chain
    /// </summary>
    public static class SendDataManager
    {
        /// <summary>
        /// Static Constructor
        /// </summary>
        static SendDataManager() => NetworkingSettings.HostSession.RegisterHandler(ReceiveSendData);
        /// <summary>
        /// Count Variable to ensure unique Channel Names
        /// </summary>
        private static int FileCount = 0;
        /// <summary>
        /// Returns the next Channel Name
        /// </summary>
        /// <returns>Unique Channel Name</returns>
        private static string GetKey() => $"FILE_" + FileCount++;
        /// <summary>
        /// All Active Send Data Events of the Client
        /// </summary>
        private static Dictionary<string, ConsoleSocket.PackageReceive> Events = new Dictionary<string, ConsoleSocket.PackageReceive>();
        /// <summary>
        /// All Active Channels that are waiting for data on the host.
        /// </summary>
        private static Dictionary<ConsoleSocket, Stream> ActiveChannels = new Dictionary<ConsoleSocket, Stream>();

        /// <summary>
        /// Tries to send the Specified File to the Connected Host
        /// </summary>
        /// <param name="file">Local Source File</param>
        /// <param name="destination">Remote Destination File</param>
        /// <returns></returns>
        public static bool TrySendData(string file, string destination) //Client Side
        {
            if (!NetworkingSettings.AllowSend || !File.Exists(file) || NetworkingSettings.ClientSession == null ||
                !NetworkingSettings.ClientSession.Client.Connected ||
                !NetworkingSettings.ClientSession.Client.IsAuthenticated)
            {
                NetworkedInitializer.Logger.LogWarning("Can not Send Data");
                return false;
            }

            string key = GetKey();
            ConsoleSocket.PackageReceive a = package => OnReceiveAllowResponse(file, destination, key, package);
            NetworkingSettings.ClientSession.Client.OnPacketReceive += a;
            Events[key] = a;
            NetworkingSettings.ClientSession.Client.TrySendPacket(new SendDataRequestPacket(destination));
            return true;
        }

        /// <summary>
        /// Gets Invoked when a Client 
        /// </summary>
        /// <param name="file">The Source File</param>
        /// <param name="destination">The Remote Destination File</param>
        /// <param name="key">The Active Send Data Key</param>
        /// <param name="p">The Packet</param>
        private static void OnReceiveAllowResponse(string file, string destination, string key, ANetworkPacket p)
        {
            if (p is SendDataRequestResponsePacket packet)
            {
                NetworkingSettings.ClientSession.Client.OnPacketReceive -= Events[key];
                Events.Remove(key);
                if (packet.Allowed)
                {
                    Task t = new Task(() => SendDataLoop(file));
                    t.Start();
                }
                else
                {
                    NetworkedInitializer.Logger.LogWarning("Host denied the Transmission");
                }
            }
        }

        /// <summary>
        /// Reads the Content of the file and sends it chunked through a series of SendDataPacket s
        /// </summary>
        /// <param name="file">Local File</param>
        private static void SendDataLoop(string file)
        {
            Stream s = File.OpenRead(file);
            int read = 0;
            byte[] buf = new byte[NetworkingSettings.PacketDataMaxSize-1];
            while ((read = s.Read(buf, 0, buf.Length)) != 0)
            {
                if (read == buf.Length)
                    NetworkingSettings.ClientSession.Client.TrySendPacket(new SendDataPacket { Data = buf });
                else
                    NetworkingSettings.ClientSession.Client.TrySendPacket(new SendDataPacket
                    { Data = buf.Take(read).ToArray(), LastPacket = true});
            }
        }

        /// <summary>
        /// Opens a Receive Channel for the Specified Client Session
        /// </summary>
        /// <param name="session">Client Session</param>
        /// <param name="destination">Destination path of the received data</param>
        public static void OpenReceiveChannel(ConsoleSocket session, string destination)
        {
            if (ActiveChannels.ContainsKey(session))
            {
                NetworkedInitializer.Logger.LogWarning("There is already a Transmission Channel Open for this Client.");
            }
            if (File.Exists(destination)) File.Delete(destination);
            ActiveChannels[session] = File.OpenWrite(destination);
        }

        /// <summary>
        /// Receives and Consumes SendDataPackets from clients.
        /// </summary>
        /// <param name="session">The Client Session</param>
        /// <param name="p">The Packet</param>
        private static void ReceiveSendData(ConsoleSocket session, ANetworkPacket p)
        {
            if (p is SendDataPacket data)
            {
                if (ActiveChannels.ContainsKey(session))
                {
                    ActiveChannels[session].Write(data.Data, 0, data.Data.Length);
                    if (data.LastPacket)
                    {
                        ActiveChannels[session].Close();
                        ActiveChannels.Remove(session);
                        NetworkedInitializer.Logger.Log("Transmission finished.");
                    }
                }
            }
        }
    }
}