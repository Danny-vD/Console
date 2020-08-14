using Console.Networking.Packets;

namespace Console.Networking.Authentication
{
    public class DefaultAuthenticator : IAuthenticator
    {
        public byte[] Decrypt(byte[] data) => data;
        public byte[] Encrypt(byte[] data) => data;

        public void AuthenticateClient(ConsoleSocket client)
        {
            client.SetAuthenticator(this);
        }
    }
}