using System;
using Console.Networking.Packets.Abstract;

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

        public override bool DoNotEncrypt => true;

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
            catch (Exception e)
            {
                return new AuthenticationPacket(new byte[Data.Length]);
            }
        }

    }
}