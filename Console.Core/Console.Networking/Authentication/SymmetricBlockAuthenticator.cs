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
    /// <summary>
    /// IAuthenticatior Implementation that works with SymmetricAlgorithms to encrypt and decrypt packets.
    /// </summary>
    public class SymmetricBlockAuthenticator : IAuthenticator
    {
        /// <summary>
        /// The Password used for authentication.
        /// </summary>
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
        /// <summary>
        /// The Encryption Algorithm
        /// </summary>
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
        /// <summary>
        /// The Hash Provider
        /// </summary>
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
        /// <summary>
        /// The Algorithm CipherMode
        /// </summary>
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

        /// <summary>
        /// The Algorithm Padding Mode
        /// </summary>
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

        /// <summary>
        /// The Size of the Encrypted Chunk of authentication data.
        /// </summary>
        [Property("networking.auth.symmetric.authdatalength")]
        public static int AuthenticationDataLength = 256;


        /// <summary>
        /// Creates the Password Used for the Algorithm
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private static byte[] CreateKey(int count)
        {
            return CreateFixSize(HashProvider.ComputeHash(NetworkingSettings.EncodingInstance.GetBytes(AuthPassword)), count);
        }

        /// <summary>
        /// Creates a Byte array of fixed size from the contents of data
        /// </summary>
        /// <param name="data">Source Data</param>
        /// <param name="count">New Array Size</param>
        /// <returns></returns>
        private static byte[] CreateFixSize(byte[] data, int count)
        {
            byte[] ret = new byte[count];
            for (int i = 0; i < count; i++)
            {
                ret[i] = data[i % data.Length];
            }
            return ret;
        }

        /// <summary>
        /// Auth Password Backing Field.
        /// </summary>
        private static string _authPassword = "12345678";
        /// <summary>
        /// Algorithm Provider Type Backing Field
        /// </summary>
        private static string _encryptionType = "AES";
        /// <summary>
        /// Hash Provider Type Backing Field.
        /// </summary>
        private static string _hashType = "SHA256";
        /// <summary>
        /// The Algorithm Cipher Mode Backing Field
        /// </summary>
        private static CipherMode _cipherMode = CipherMode.CBC;
        /// <summary>
        /// The Algorithm Padding Mode Backing Field
        /// </summary>
        private static PaddingMode _paddingMode = PaddingMode.PKCS7;


        /// <summary>
        /// Hash Provider Backing Field.
        /// </summary>
        private static HashAlgorithm hashProvider;
        /// <summary>
        /// Hash Provider
        /// </summary>
        private static HashAlgorithm HashProvider => hashProvider ?? (hashProvider = HashAlgorithm.Create(HashType));
        /// <summary>
        /// Algorithm Provider Backing Field.
        /// </summary>
        private static SymmetricAlgorithm provider;
        /// <summary>
        /// Algorithm Provider
        /// </summary>
        private static SymmetricAlgorithm Provider => provider ?? (provider = CreateProvider());

        /// <summary>
        /// Collection of Authentication Sessions
        /// </summary>
        private static Dictionary<ConsoleSocket, byte[]> AuthenticationSessions = new Dictionary<ConsoleSocket, byte[]>();

        /// <summary>
        /// Creates the Specified Symmetric Algorithm
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets Invoked by the Host to initialize the authentication of a connected client.
        /// </summary>
        /// <param name="client">Client to Authenticate</param>
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

        /// <summary>
        /// Gets invoked when the Host Receives an Authentication Packet.
        /// This Functions is Checking for the clients Authority.
        /// </summary>
        /// <param name="client">Client to Authorize</param>
        /// <param name="package">The Authentication Packet</param>
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

            NetworkedInitializer.Logger.Log("Authenticated: " + suc);

            client.TrySendPacket(new AuthenticationResultPacket(suc));
            AuthenticationSessions.Remove(client);
        }

        /// <summary>
        /// Returns a Byte array into a string.
        /// </summary>
        /// <param name="data">Data to be converted</param>
        /// <returns>The String representation of the data array.</returns>
        private string GetText(byte[] data)
        {
            string s = "";
            for (int i = 0; i < data.Length; i++)
            {
                s += data[i];
            }
            return s;
        }

        /// <summary>
        /// Returns True if the Content of the arrays is the same.
        /// </summary>
        /// <param name="original">Array to test Against</param>
        /// <param name="data">Array to Test</param>
        /// <returns>True if Equal</returns>
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

        /// <summary>
        /// Returns an Array of Random Data.
        /// </summary>
        /// <param name="len">Length of the Array</param>
        /// <returns>Array with random numbers</returns>
        private byte[] GetRandomData(int len)
        {
            Random rnd = new Random();
            byte[] ret = new byte[len];
            rnd.NextBytes(ret);
            return ret;
        }



        /// <summary>
        /// Decrypts the Passed Data
        /// </summary>
        /// <param name="data">Passed Data</param>
        /// <returns>Decrypted Data</returns>
        public byte[] Decrypt(byte[] data)
        {
            return Cryptography.Decrypt(Provider, data, CreateKey(Provider.Key.Length));
        }

        /// <summary>
        /// Encrypts the Passed Data
        /// </summary>
        /// <param name="data">Passed Data</param>
        /// <returns>Encrypted Data</returns>
        public byte[] Encrypt(byte[] data)
        {
            return Cryptography.Encrypt(Provider, data, CreateKey(Provider.Key.Length));
        }

        //Client => ConnectionRequest
        //Server => RequestResponse + Data To encrypt
        //Client => AuthPacket + EncryptedData
    }
}