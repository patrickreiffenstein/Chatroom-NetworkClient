using System;
using System.Text;
using static Chatroom.NetworkClient.NetworkClient;

namespace Chatroom.NetworkClient.ClientPackets
{
    /// <summary>
    /// Pakken som bruges til at sende beskeder.
    /// </summary>
    public class SendMessagePacket : ClientPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendMessagePacket"/> class.
        /// </summary>
        /// <param name="message"> Beskeden der skal sendes. </param>
        /// <param name="id"> ID på den som skal modtage beskeden. </param>
        public SendMessagePacket(string message, byte id)
        {
            Bytes = new byte[sizeof(byte) + sizeof(byte) + sizeof(ushort) + Encoding.UTF8.GetByteCount(message)];

            // PackageID
            Bytes[0] = (byte)Packets.SendMessage;

            // UserID
            Bytes[1] = (byte)id;

            // MessageLength
            byte[] messageLengthBytes = BitConverter.GetBytes((ushort)Encoding.UTF8.GetByteCount(message));

            for (int i = 0; i < messageLengthBytes.Length; i++)
            {
                Bytes[2 + i] = messageLengthBytes[i];
            }

            // Message
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            for (int i = 0; i < messageBytes.Length; i++)
            {
                Bytes[4 + i] = messageBytes[i];
            }
        }
    }
}