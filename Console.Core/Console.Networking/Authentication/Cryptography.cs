using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Console.Core.PropertySystem;

namespace Console.Networking.Authentication
{
    public static class Cryptography
    {
        #region Settings

        
        [Property("networking.auth.symmetric.salt")]
        public static string DefaultSalt = "aselrias38490a32"; // Random
        [Property("networking.auth.symmetric.vector")]
        public static string DefaultVector = "8947az34awl34kjq"; // Random

        static Cryptography()
        {
            Random rnd = new Random();
        }

        private static string GetRandom(Random rnd, int size)
        {
            byte[] data = new byte[size];
            rnd.NextBytes(data);
            return Convert.ToBase64String(data);
        }

        #endregion

        #region Password Derive Bytes

        public static Rfc2898DeriveBytes GetPassword(byte[] pass, byte[] salt, int iterations = 2)
        {
            return new Rfc2898DeriveBytes(pass, salt, iterations);
        }


        #endregion

        #region Encrypt

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

        public static byte[] Encrypt(SymmetricAlgorithm alg, byte[] value, byte[] password, byte[] salt, byte[] vector)
        {
            return Encrypt(alg, value, vector, GetPassword(password, salt));
        }

        public static byte[] Encrypt<T>(byte[] value, byte[] password, byte[] salt, byte[] vector)
            where T : SymmetricAlgorithm, new()
        {
            return Encrypt(new T(), value, password, salt, vector);
        }

        public static byte[] Encrypt(SymmetricAlgorithm alg, byte[] value, byte[] password)
        {
            return Encrypt(alg, value, password, GetBytes<ASCIIEncoding>(DefaultSalt), GetBytes<ASCIIEncoding>(DefaultVector));
        }
        public static byte[] Encrypt<T>(byte[] value, byte[] password)
            where T : SymmetricAlgorithm, new()
        {
            return Encrypt<T>(value, password, GetBytes<ASCIIEncoding>(DefaultSalt), GetBytes<ASCIIEncoding>(DefaultVector));
        }


        #endregion

        #region Decrypt

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

        public static byte[] Decrypt(SymmetricAlgorithm alg, byte[] value, byte[] password, byte[] salt, byte[] vector)
        {
            return Decrypt(alg, value, vector, GetPassword(password, salt));
        }

        public static byte[] Decrypt<T>(byte[] value, byte[] password, byte[] salt, byte[] vector) where T : SymmetricAlgorithm, new()
        {
            return Decrypt(new T(), value, password, salt, vector);
        }

        public static byte[] Decrypt(SymmetricAlgorithm alg, byte[] value, byte[] password)
        {
            return Decrypt(alg, value, password, GetBytes<ASCIIEncoding>(DefaultSalt), GetBytes<ASCIIEncoding>(DefaultVector));
        }
        public static byte[] Decrypt<T>(byte[] value, byte[] password)
            where T : SymmetricAlgorithm, new()
        {
            return Decrypt<T>(value, password, GetBytes<ASCIIEncoding>(DefaultSalt), GetBytes<ASCIIEncoding>(DefaultVector));
        }


        #endregion

        #region GetBytes

        public static byte[] GetBytes<T>(string data) where T : Encoding, new()
        {
            return GetBytes(new T(), data);
        }

        public static byte[] GetBytes(Encoding enc, string data)
        {
            return enc.GetBytes(data);
        }

        #endregion
    }
}