using Console.Networking.Packets;

namespace Console.Networking.Authentication
{
    /// <summary>
    /// IAuthenticator Instances Implement Encryption and Security Features.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Gets Invoked by the Host to initialize the authentication of a connected client.
        /// </summary>
        /// <param name="client">Client to Authenticate</param>
        void AuthenticateClient(ConsoleSocket client);
        /// <summary>
        /// Decrypts the Passed Data
        /// </summary>
        /// <param name="data">Passed Data</param>
        /// <returns>Decrypted Data</returns>
        byte[] Decrypt(byte[] data);
        /// <summary>
        /// Encrypts the Passed Data
        /// </summary>
        /// <param name="data">Passed Data</param>
        /// <returns>Encrypted Data</returns>
        byte[] Encrypt(byte[] data);
    }
}