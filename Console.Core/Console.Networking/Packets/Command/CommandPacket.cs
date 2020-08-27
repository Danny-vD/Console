using Console.Networking.Packets.Abstract;


/// <summary>
/// CommandPacket Classes
/// </summary>
namespace Console.Networking.Packets.Command
{
    /// <summary>
    /// Gets Sent from the client to the server to execute a command.
    /// </summary>
    public class CommandPacket : ANetworkPacket
    {
        /// <summary>
        /// Flag that determines if the Command was resolved by the client.
        /// </summary>
        public readonly bool Resolved;

        /// <summary>
        /// The Client Input.
        /// </summary>
        public readonly string Input;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="resolved">Determines if the Command was resolved by the client</param>
        /// <param name="input">The input string,</param>
        public CommandPacket(bool resolved, string input)
        {
            Resolved = resolved;
            Input = input;
        }
    }
}