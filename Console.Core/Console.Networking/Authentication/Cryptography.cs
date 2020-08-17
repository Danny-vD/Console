using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Console.Core.PropertySystem;


/// <summary>
/// Namespace of the Authentication System
/// </summary>
namespace Console.Networking.Authentication
{
    /// <summary>
    /// Helper Class that Implements Symmetric Algorithm Functions
    /// </summary>
    public static class Cryptography
    {
        #region Settings

        /// <summary>
        /// The Password Salt
        /// </summary>
        [Property("networking.auth.symmetric.salt")]
        public static string DefaultSalt = "aselrias38490a32"; // Random

        /// <summary>
        /// The Initialization Vector(IV)
        /// </summary>
        [Property("networking.auth.symmetric.vector")]
        public static string DefaultVector = "8947az34awl34kjq"; // Random

        /// <summary>
        /// Returns a Random string that is size characters long
        /// </summary>
        /// <param name="rnd">Random Ínstance</param>
        /// <param name="size">The Size of the Resulting String</param>
        /// <returns>The Generated Random String Sequence</returns>
        private static string GetRandom(Random rnd, int size)
        {
            byte[] data = new byte[size];
            rnd.NextBytes(data);
            return Convert.ToBase64String(data);
        }

        #endregion

        #region Password Derive Bytes
        /// <summary>
        /// Creates a Rfc2898DeriveBytes Instance from a password.
        /// </summary>
        /// <param name="pass">Password to use.</param>
        /// <param name="salt">The Password Salt.</param>
        /// <param name="iterations">How many iterations should the Derrive Algorithm Perform.</param>
        /// <returns></returns>
        public static Rfc2898DeriveBytes GetPassword(byte[] pass, byte[] salt, int iterations = 2)
        {
            return new Rfc2898DeriveBytes(pass, salt, iterations);
        }


        #endregion

        #region Encrypt

        /// <summary>
        /// Encrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <param name="alg">Algorithm to Use</param>
        /// <param name="value">The Value to Encrypt</param>
        /// <param name="vector">The Initialization Vector(IV)</param>
        /// <param name="password">The Password.</param>
        /// <returns>Encrypted Value</returns>
        public static byte[] Encrypt(SymmetricAlgorithm alg, byte[] value, byte[] vector, Rfc2898DeriveBytes password)
        {

            byte[] encrypted;

            byte[] keyBytes = password.GetBytes(alg.KeySize / 8);

            alg.Mode = CipherMode.CBC;

            using (ICryptoTransform encryptor = alg.CreateEncryptor(keyBytes, vector))
            {
                using (MemoryStream to = new MemoryStream())
                {
                    using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                    {
                        writer.Write(value, 0, value.Length);
                        writer.FlushFinalBlock();
                        encrypted = to.ToArray();
                    }
                }
            }

            return encrypted;
        }
        /// <summary>
        /// Encrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <param name="alg">Algorithm to Use</param>
        /// <param name="value">The Value to Encrypt</param>
        /// <param name="password">The Password.</param>
        /// <param name="salt">The password Salt</param>
        /// <param name="vector">The Initialization Vector(IV)</param>
        /// <returns>Encrypted Value</returns>
        public static byte[] Encrypt(SymmetricAlgorithm alg, byte[] value, byte[] password, byte[] salt, byte[] vector)
        {
            return Encrypt(alg, value, vector, GetPassword(password, salt));
        }
        /// <summary>
        /// Encrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <typeparam name="T">The Algorithm to Use.</typeparam>
        /// <param name="value">The Value to Encrypt</param>
        /// <param name="password">The Password.</param>
        /// <param name="salt">The password Salt</param>
        /// <param name="vector">The Initialization Vector(IV)</param>
        /// <returns>Encrypted Value</returns>
        public static byte[] Encrypt<T>(byte[] value, byte[] password, byte[] salt, byte[] vector)
            where T : SymmetricAlgorithm, new()
        {
            return Encrypt(new T(), value, password, salt, vector);
        }

        /// <summary>
        /// Encrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <param name="alg">Algorithm to Use</param>
        /// <param name="value">The Value to Encrypt</param>
        /// <param name="password">The Password.</param>
        /// <returns>Encrypted Value</returns>
        public static byte[] Encrypt(SymmetricAlgorithm alg, byte[] value, byte[] password)
        {
            return Encrypt(alg, value, password, GetBytes(NetworkingSettings.EncodingInstance,DefaultSalt), GetBytes(NetworkingSettings.EncodingInstance,DefaultVector));
        }
        /// <summary>
        /// Encrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <typeparam name="T">The Algorithm to Use.</typeparam>
        /// <param name="value">The Value to Encrypt</param>
        /// <param name="password">The Password.</param>
        /// <returns>Encrypted Value</returns>
        public static byte[] Encrypt<T>(byte[] value, byte[] password)
            where T : SymmetricAlgorithm, new()
        {
            return Encrypt<T>(value, password, GetBytes(NetworkingSettings.EncodingInstance,DefaultSalt), GetBytes(NetworkingSettings.EncodingInstance,DefaultVector));
        }


        #endregion

        #region Decrypt
        /// <summary>
        /// Decrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <param name="alg">Algorithm to Use</param>
        /// <param name="value">The Value to Decrypt</param>
        /// <param name="vector">The Initialization Vector(IV)</param>
        /// <param name="password">The Password.</param>
        /// <returns>Decrypted Value</returns>
        public static byte[] Decrypt(SymmetricAlgorithm alg, byte[] value, byte[] vector, Rfc2898DeriveBytes password)
        {
            byte[] valueBytes = value;

            byte[] decrypted;
            int decryptedByteCount = 0;

            byte[] keyBytes = password.GetBytes(alg.KeySize / 8);

            alg.Mode = CipherMode.CBC;

            try
            {
                using (ICryptoTransform decryptor = alg.CreateDecryptor(keyBytes, vector))
                {
                    using (MemoryStream from = new MemoryStream(valueBytes))
                    {
                        using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                        {
                            decrypted = new byte[valueBytes.Length];
                            decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return decrypted.Take(decryptedByteCount).ToArray();
        }


        /// <summary>
        /// Decrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <param name="alg">Algorithm to Use</param>
        /// <param name="value">The Value to Decrypt</param>
        /// <param name="password">The Password.</param>
        /// <param name="salt">The Password Salt.</param>
        /// <param name="vector">The Initialization Vector(IV)</param>
        /// <returns>Decrypted Value</returns>
        public static byte[] Decrypt(SymmetricAlgorithm alg, byte[] value, byte[] password, byte[] salt, byte[] vector)
        {
            return Decrypt(alg, value, vector, GetPassword(password, salt));
        }

        /// <summary>
        /// Decrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <typeparam name="T">Algorithm to Use</typeparam>
        /// <param name="value">The Value to Decrypt</param>
        /// <param name="password">The Password.</param>
        /// <param name="salt">The Password Salt.</param>
        /// <param name="vector">The Initialization Vector(IV)</param>
        /// <returns>Decrypted Value</returns>
        public static byte[] Decrypt<T>(byte[] value, byte[] password, byte[] salt, byte[] vector) where T : SymmetricAlgorithm, new()
        {
            return Decrypt(new T(), value, password, salt, vector);
        }
        /// <summary>
        /// Decrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <param name="alg">Algorithm to Use</param>
        /// <param name="value">The Value to Decrypt</param>
        /// <param name="password">The Password.</param>
        /// <returns>Decrypted Value</returns>
        public static byte[] Decrypt(SymmetricAlgorithm alg, byte[] value, byte[] password)
        {
            return Decrypt(alg, value, password, GetBytes(NetworkingSettings.EncodingInstance, DefaultSalt), GetBytes(NetworkingSettings.EncodingInstance,DefaultVector));
        }

        /// <summary>
        /// Decrypts a Value with a Symmetric Algorithm
        /// </summary>
        /// <typeparam name="T">Algorithm to Use</typeparam>
        /// <param name="value">The Value to Decrypt</param>
        /// <param name="password">The Password.</param>
        /// <returns>Decrypted Value</returns>
        public static byte[] Decrypt<T>(byte[] value, byte[] password)
            where T : SymmetricAlgorithm, new()
        {
            return Decrypt<T>(value, password, GetBytes(NetworkingSettings.EncodingInstance, DefaultSalt), GetBytes(NetworkingSettings.EncodingInstance, DefaultVector));
        }


        #endregion

        #region GetBytes

        /// <summary>
        /// Converts a String into a Byte Array.
        /// </summary>
        /// <typeparam name="T">The Encoding</typeparam>
        /// <param name="data">The data to convert.</param>
        /// <returns>Encoded String</returns>
        public static byte[] GetBytes<T>(string data) where T : Encoding, new()
        {
            return GetBytes(new T(), data);
        }
        /// <summary>
        /// Converts a String into a Byte Array.
        /// </summary>
        /// <param name="enc">The Encoding</param>
        /// <param name="data">The data to convert.</param>
        /// <returns>Encoded String</returns>
        public static byte[] GetBytes(Encoding enc, string data)
        {
            return enc.GetBytes(data);
        }

        #endregion
    }
}