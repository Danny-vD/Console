using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Console.Core;
using Console.Core.PropertySystem;
using Console.Networking.Packets;
using Console.Networking.Packets.Authentication;
using Console.Networking.Packets.AuthenticationResult;
using Console.Networking.Packets.ConnectionAbort;

namespace Console.Networking.Authentication
{
    public class SymmetricBlockAuthenticator : IAuthenticator
    {
        [Property("networking.auth.symmetric.password")]
        public static string AuthPassword
        {
            get => _authPassword;
            set
            {
                _authPassword = value;
                CreateProvider();
            }
        }
        [Property("networking.auth.symmetric.algorithm")]
        public static string EncryptionType
        {
            get => _encryptionType;
            set
            {
                _encryptionType = value;
                CreateProvider();
            }
        }
        [Property("networking.auth.symmetric.hash")]
        public static string HashType
        {
            get => _hashType;
            set
            {
                _hashType = value;
                CreateProvider();
            }
        }
        [Property("networking.auth.symmetric.ciphermode")]
        public static CipherMode CipherMode
        {
            get => _cipherMode;
            set
            {
                _cipherMode = value;
                Provider.Mode = value;
            }
        }
        [Property("networking.auth.symmetric.paddingmode")]
        public static PaddingMode PaddingMode
        {
            get => _paddingMode;
            set
            {
                _paddingMode = value;
                Provider.Padding = value;
            }
        }


        [Property("networking.auth.symmetric.authdatalength")]
        public static int AuthenticationDataLength = 256;


        private static byte[] CreateKey(int count)
        {
            return CreateFixSize(HashProvider.ComputeHash(NetworkingSettings.EncodingInstance.GetBytes(AuthPassword)), count);
        }

        private static byte[] CreateFixSize(byte[] data, int count)
        {
            byte[] ret = new byte[count];
            for (int i = 0; i < count; i++)
            {
                ret[i] = data[i % data.Length];
            }
            return ret;
        }

        private static string _authPassword = "12345678";
        private static string _encryptionType = "AES";
        private static string _hashType = "SHA256";
        private static CipherMode _cipherMode = CipherMode.CBC;
        private static PaddingMode _paddingMode = PaddingMode.PKCS7;


        private static HashAlgorithm hashProvider;
        private static HashAlgorithm HashProvider => hashProvider ?? (hashProvider = HashAlgorithm.Create(HashType));
        private static SymmetricAlgorithm provider;
        private static SymmetricAlgorithm Provider => provider ?? (provider = CreateProvider());

        private static Dictionary<ConsoleSocket, byte[]> AuthenticationSessions = new Dictionary<ConsoleSocket, byte[]>();

        private static SymmetricAlgorithm CreateProvider()
        {
            provider = SymmetricAlgorithm.Create(EncryptionType);
            hashProvider = HashAlgorithm.Create(HashType);
            //provider.Mode = CipherMode;
            //provider.Padding = PaddingMode;
            provider.Key = CreateKey(provider.Key.Length);
            provider.GenerateIV();
            return provider;
        }

        public void AuthenticateClient(ConsoleSocket client)
        {

            byte[] data = GetRandomData(AuthenticationDataLength);


            AuthenticationSessions[client] = data;

            client.OnPacketReceive += package =>
            {
                if (package is AuthenticationPacket a) ClientAuthenticationReceive(client, a);
            };
            if (!client.TrySendPacket(
                new AuthenticationPacket(Encrypt(data))))
            {
                return;
            }

        }

        private void ClientAuthenticationReceive(ConsoleSocket client, AuthenticationPacket package)
        {
            bool suc = AuthenticationSessions.ContainsKey(client) &&
                       IsEqual(AuthenticationSessions[client], package.Data);
            if (suc)
            {
                client.SetAuthenticator(this);
            }
            else
            {
                client.TrySendPacket(new ConnectionAbortPacket("Invalid Password."));
            }

            AConsoleManager.Instance.Log("Authenticated: " + suc);

            client.TrySendPacket(new AuthenticationResultPacket(suc));
            AuthenticationSessions.Remove(client);
        }

        private string GetText(byte[] data)
        {
            string s = "";
            for (int i = 0; i < data.Length; i++)
            {
                s += data[i];
            }
            return s;
        }

        private bool IsEqual(byte[] original, byte[] data)
        {
            if (original == null && data == null) return true;
            if ((original == null) || (data == null)) return false;
            //if (original != data) return false;
            if (original.Length != data.Length) return false;
            for (int i = 0; i < original.Length; i++)
            {
                if (original[i] != data[i]) return false;
            }
            return true;
        }

        private byte[] GetRandomData(int len)
        {
            Random rnd = new Random();
            byte[] ret = new byte[len];
            rnd.NextBytes(ret);
            return ret;
        }



        public byte[] Decrypt(byte[] data)
        {
            return Cryptography.Decrypt(Provider, data, CreateKey(Provider.Key.Length));
        }

        public byte[] Encrypt(byte[] data)
        {
            return Cryptography.Encrypt(Provider, data, CreateKey(Provider.Key.Length));
        }

        //Client => ConnectionRequest
        //Server => RequestResponse + Data To encrypt
        //Client => AuthPacket + EncryptedData
    }
}