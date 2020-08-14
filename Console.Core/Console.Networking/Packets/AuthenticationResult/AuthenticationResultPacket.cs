using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.AuthenticationResult
{
    /// <summary>
    /// Host Response on the Authentication State
    /// </summary>
    public class AuthenticationResultPacket : ANetworkPacket
    {
        public readonly bool Success;

        public AuthenticationResultPacket(bool success)
        {
            Success = success;
        }
    }
}