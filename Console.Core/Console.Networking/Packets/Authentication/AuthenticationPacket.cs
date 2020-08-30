using System;
using Console.Networking.Packets.Abstract;

/// <summary>
/// AuthenticationPacket Classes
/// </summary>
namespace Console.Networking.Packets.Authentication
{
    /// <summary>
    /// Gets Send by the Host with a Encrypted Data Section
    /// To Authenitcate send the Encrypted Data Section back to the host.
    /// </summary>
    public class AuthenticationPacket : ANetworkPacket
    {
        /// <summary>
        /// The Data Section
        /// </summary>
        public readonly byte[] Data;


        /// <summary>
        /// Flag that specifies that the networking layer should not encrypt/decrypt the packet.
        /// </summary>
        public override bool DoNotEncrypt => true;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="data"></param>
        public AuthenticationPacket(byte[] data)
        {
            Data = data;
        }

        /// <summary>
        /// Tries to Create the AuthenticationPacket Response with the Decrypted Data
        /// </summary>
        /// <returns></returns>
        public AuthenticationPacket CreateResponse()
        {
            //Decrypt the Data
            try
            {

                return new AuthenticationPacket(NetworkingSettings.AuthenticatorInstance.Decrypt(Data));
            }
            catch (Exception)
            {
                return new AuthenticationPacket(new byte[Data.Length]);
            }
        }
    }
}