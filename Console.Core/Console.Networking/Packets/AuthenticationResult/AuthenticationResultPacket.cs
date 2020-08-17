using Console.Networking.Packets.Abstract;



/// <summary>
/// AuthenticationResultPacket Classes
/// </summary>
namespace Console.Networking.Packets.AuthenticationResult
{
    /// <summary>
    /// Host Response on the Authentication State
    /// </summary>
    public class AuthenticationResultPacket : ANetworkPacket
    {
        /// <summary>
        /// Flag that Indicates the State of the Authentication
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="success">Authentication State</param>
        public AuthenticationResultPacket(bool success)
        {
            Success = success;
        }
    }
}