using Console.Networking.Packets;

namespace Console.Networking.Authentication
{
    /// <summary>
    /// The Default Authenticator.
    /// Does not Authenticate.
    /// Does not Encrypt/Decrypt.
    /// </summary>
    public class DefaultAuthenticator : IAuthenticator
    {
        /// <summary>
        /// Decrypts the Passed Data
        /// </summary>
        /// <param name="data">Passed Data</param>
        /// <returns>Decrypted Data</returns>
        public byte[] Decrypt(byte[] data) => data;
        /// <summary>
        /// Encrypts the Passed Data
        /// </summary>
        /// <param name="data">Passed Data</param>
        /// <returns>Encrypted Data</returns>
        public byte[] Encrypt(byte[] data) => data;

        /// <summary>
        /// Gets Invoked by the Host to initialize the authentication of a connected client.
        /// </summary>
        /// <param name="client">Client to Authenticate</param>
        public void AuthenticateClient(ConsoleSocket client)
        {
            client.SetAuthenticator(this);
        }
    }
}