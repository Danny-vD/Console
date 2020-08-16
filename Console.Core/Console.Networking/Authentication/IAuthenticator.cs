using Console.Networking.Packets;

namespace Console.Networking.Authentication
{
    public interface IAuthenticator
    {
        void AuthenticateClient(ConsoleSocket client);
        byte[] Decrypt(byte[] data);
        byte[] Encrypt(byte[] data);
    }
}