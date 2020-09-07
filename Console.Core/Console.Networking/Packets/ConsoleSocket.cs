﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

using Console.Networking.Authentication;
using Console.Networking.Packets.Abstract;

/// <summary>
/// Namespace Containing all Packets in the Networking Extension
/// </summary>
namespace Console.Networking.Packets
{
    /// <summary>
    /// Networking Socket for the ConsoleSystem.
    /// </summary>
    public class ConsoleSocket : IDisposable
    {

        /// <summary>
        /// Delegate that gets invoked when a Packet Gets Received on this ConsoleSocket
        /// </summary>
        /// <param name="package">The Received package</param>
        public delegate void PackageReceive(ANetworkPacket package);

        /// <summary>
        /// Authenticator Instance Backing Field.
        /// </summary>
        private IAuthenticator _authenticator;

        /// <summary>
        /// The Underlying TCP Client.
        /// </summary>
        private TcpClient Client;

        /// <summary>
        /// Public Constructor
        /// </summary>
        public ConsoleSocket()
        {
        }

        /// <summary>
        /// Public Construtor
        /// </summary>
        /// <param name="client">Underlying TCP Client</param>
        public ConsoleSocket(TcpClient client)
        {
            Client = client;
        }

        /// <summary>
        /// True if the Underlying Resources are disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// True if the Client is Still Connected
        /// </summary>
        public bool Connected => Client?.Client != null && Client.Connected;

        /// <summary>
        /// True if the IAuthenticator Instance authorized the Client
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Authenticator Instance
        /// </summary>
        private IAuthenticator Authenticator => _authenticator ?? NetworkingSettings.AuthenticatorInstance;

        /// <summary>
        /// Disposes all underlying resources.
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
            OnPacketReceive = null;
            IsDisposed = true;
        }

        /// <summary>
        /// Event that gets invoked when a Packet gets received.
        /// </summary>
        public event PackageReceive OnPacketReceive;

        /// <summary>
        /// Sets the Authenticator Instance
        /// </summary>
        /// <param name="auth">New IAuthenticator Instance</param>
        public void SetAuthenticator(IAuthenticator auth)
        {
            _authenticator = auth;
            IsAuthenticated = true;
        }

        /// <summary>
        /// Connects to the Specified Host
        /// </summary>
        /// <param name="ip">Host IP</param>
        /// <param name="port">Host Port</param>
        public void Connect(string ip, int port)
        {
            Client?.Dispose();
            Client = new TcpClient(ip, port);
        }

        /// <summary>
        /// Tries to send a packet through this socket.
        /// </summary>
        /// <param name="packet">Packet to Send</param>
        /// <returns>True if the Packet got serialized and sent.</returns>
        public bool TrySendPacket(ANetworkPacket packet)
        {
            if (SerializerCollection.CanSerialize(packet.PacketIdentifier))
            {
                List<byte> data = new List<byte>();

                data.Add((byte) (packet.DoNotEncrypt ? 1 : 0)); //DoNotEncryptFlag

                byte[] idBuf = NetworkingSettings.EncodingInstance.GetBytes(packet.PacketIdentifier);
                if (!packet.DoNotEncrypt)
                {
                    idBuf = Authenticator.Encrypt(idBuf);
                }

                data.AddRange(BitConverter.GetBytes(idBuf.Length)); //Packet Identifier Length
                data.AddRange(idBuf); //Packet Identifier

                byte[] dData = SerializerCollection.Serialize(packet);
                if (!packet.DoNotEncrypt)
                {
                    dData = Authenticator.Encrypt(dData);
                }

                data.AddRange(BitConverter.GetBytes(dData.Length)); //Packet Data Length
                data.AddRange(dData); //Packet Data

                Client.GetStream().Write(data.ToArray(), 0, data.Count);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Processes the Packets that were received.
        /// </summary>
        public void ProcessPacket()
        {
            if (Client?.Client == null || !Client.Connected)
            {
                return;
            }

            if (Client.Available > sizeof(int) + 1)
            {
                bool dec = Client.GetStream().ReadByte() == 0; //DoNotEncrypt Flag

                byte[] lenBuf = new byte[sizeof(int)];
                Client.GetStream().Read(lenBuf, 0, lenBuf.Length); //Packet Identifier Length
                int len = BitConverter.ToInt32(lenBuf, 0);

                if (len <= 0 || len > NetworkingSettings.PacketIdentifierMaxBytes)
                {
                    NetworkedInitializer.Logger.LogWarning(
                                                           "Network Package Read Error: Packet Identifier size is not valid: " +
                                                           len
                                                          );
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
                {
                    idBuf = Authenticator.Decrypt(idBuf);
                }

                string id = NetworkingSettings.EncodingInstance.GetString(idBuf, 0, idBuf.Length);


                if (SerializerCollection.CanDeserialize(id))
                {
                    byte[] dataLenBuf = new byte[sizeof(int)];
                    Client.GetStream().Read(dataLenBuf, 0, dataLenBuf.Length); //Packet Data Length
                    int dataLen = BitConverter.ToInt32(dataLenBuf, 0);
                    if (dataLen <= 0 || dataLen > NetworkingSettings.PacketDataMaxBytes)
                    {
                        NetworkedInitializer.Logger.LogWarning(
                                                               "Network Package Read Error: Packet Data size is not valid: " +
                                                               dataLen
                                                              );
                        FlushNetworkStream();
                        return;
                    }

                    while (dataLen > Client.Available)
                    {
                        Thread.Sleep(NetworkingSettings.PacketWaitSleepTimer);
                    }

                    byte[] data = new byte[dataLen];


                    Client.GetStream().Read(data, 0, data.Length);
                    if (dec)
                    {
                        data = Authenticator.Decrypt(data);
                    }

                    ANetworkPacket packet = SerializerCollection.Deserialize<ANetworkPacket>(id, data);
                    OnPacketReceive?.Invoke(packet);
                }
                else
                {
                    if (!NetworkingSettings.MuteLayerLogs)
                    {
                        NetworkedInitializer.Logger.LogWarning("Can not Deserialize Packet: " + id);
                    }

                    FlushNetworkStream();
                }
            }
        }

        /// <summary>
        /// Removes all remaining bytes from the Connection Buffer
        /// </summary>
        private void FlushNetworkStream()
        {
            byte[] data = new byte[Client.Available];
            Client.GetStream().Read(data, 0, data.Length);
            NetworkedInitializer.Logger.LogWarning($"Flushed {data.Length} Bytes.");
        }

        /// <summary>
        /// To String Implementation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Client?.Client?.RemoteEndPoint?.ToString() ?? "Client not Connected";
        }

    }
}