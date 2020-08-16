using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Console.Core;
using Console.Networking.Authentication;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets
{
    public class ConsoleSocket : IDisposable
    {

        public delegate void PackageReceive(ANetworkPacket package);

        public bool IsDisposed { get; private set; }
        public bool Connected => Client?.Client != null && Client.Connected;
        public bool IsAuthenticated { get; private set; }
        public event PackageReceive OnPacketReceive;
        private TcpClient Client;
        private IAuthenticator _authenticator;
        private IAuthenticator Authenticator => _authenticator ?? (NetworkingSettings.AuthenticatorInstance);

        public ConsoleSocket() { }

        public ConsoleSocket(TcpClient client)
        {
            Client = client;
        }

        public void SetAuthenticator(IAuthenticator auth)
        {
            _authenticator = auth;
            IsAuthenticated = true;
        }

        public void Dispose()
        {
            Client.Dispose();
            OnPacketReceive = null;
            IsDisposed = true;
        }

        public void Connect(string ip, int port)
        {
            Client?.Dispose();
            Client = new TcpClient(ip, port);
        }

        public bool TrySendPacket(ANetworkPacket packet)
        {
            if (SerializerCollection.CanSerialize(packet.PacketIdentifier))
            {

                List<byte> data = new List<byte>();

                data.Add((byte)(packet.DoNotEncrypt ? 1 : 0)); //DoNotEncryptFlag

                byte[] idBuf = NetworkingSettings.EncodingInstance.GetBytes(packet.PacketIdentifier);
                if (!packet.DoNotEncrypt)
                    idBuf = Authenticator.Encrypt(idBuf);
                data.AddRange(BitConverter.GetBytes(idBuf.Length)); //Packet Identifier Length
                data.AddRange(idBuf); //Packet Identifier

                byte[] dData = SerializerCollection.Serialize(packet);
                if (!packet.DoNotEncrypt)
                    dData = Authenticator.Encrypt(dData);
                data.AddRange(BitConverter.GetBytes(dData.Length)); //Packet Data Length
                data.AddRange(dData); //Packet Data
                Client.GetStream().Write(data.ToArray(), 0, data.Count);
                return true;
            }
            return false;
        }

        public void ProcessPacket()
        {
            if (Client?.Client == null || !Client.Connected) return;
            if (Client.Available > sizeof(int) + 1)
            {
                bool dec = Client.GetStream().ReadByte() == 0; //DoNotEncrypt Flag

                byte[] lenBuf = new byte[sizeof(int)];
                Client.GetStream().Read(lenBuf, 0, lenBuf.Length); //Packet Identifier Length
                int len = BitConverter.ToInt32(lenBuf, 0);

                if (len <= 0 || len > NetworkingSettings.PacketIdentifierMaxSize)
                {
                    AConsoleManager.Instance.LogWarning("Network Package Read Error: Packet Identifier size is not valid: " + len);
                    FlushNetworkStream();
                    return;
                }
                while (len > Client.Available)
                {
                    Thread.Sleep(NetworkingSettings.PacketWaitSleepTimer);
                }

                byte[] idBuf = new byte[len];
                Client.GetStream().Read(idBuf, 0, idBuf.Length); //Packet Identifier
                if (dec)
                    idBuf = Authenticator.Decrypt(idBuf);
                string id = NetworkingSettings.EncodingInstance.GetString(idBuf, 0, idBuf.Length);

                if (SerializerCollection.CanDeserialize(id))
                {
                    byte[] dataLenBuf = new byte[sizeof(int)];
                    Client.GetStream().Read(dataLenBuf, 0, dataLenBuf.Length); //Packet Data Length
                    int dataLen = BitConverter.ToInt32(dataLenBuf, 0);
                    if (dataLen <= 0 || dataLen > NetworkingSettings.PacketDataMaxSize)
                    {
                        AConsoleManager.Instance.LogWarning("Network Package Read Error: Packet Data size is not valid: " + dataLen);
                        FlushNetworkStream();
                        return;
                    }
                    while (dataLen > Client.Available)
                    {
                        Thread.Sleep(NetworkingSettings.PacketWaitSleepTimer);
                    }

                    byte[] data = new byte[dataLen];

                    Client.GetStream().Read(data, 0, data.Length);
                    if (dec) data = Authenticator.Decrypt(data);

                    ANetworkPacket packet = SerializerCollection.Deserialize<ANetworkPacket>(id, data);
                    OnPacketReceive?.Invoke(packet);
                }
                else
                {
                    AConsoleManager.Instance.LogWarning("Can not Deserialize Packet: " + id);
                    FlushNetworkStream();
                }
            }
        }

        private void FlushNetworkStream()
        {
            byte[] data = new byte[Client.Available];
            Client.GetStream().Read(data, 0, data.Length);
            AConsoleManager.Instance.LogWarning($"Flushed {data.Length} Bytes.");
        }

        public override string ToString()
        {
            return Client?.Client?.RemoteEndPoint?.ToString() ?? $"Client not Connected";
        }
    }
}