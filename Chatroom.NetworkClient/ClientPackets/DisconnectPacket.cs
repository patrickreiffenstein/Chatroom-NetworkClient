using static Chatroom.NetworkClient.NetworkClient;

namespace Chatroom.NetworkClient.ClientPackets
{
    /// <summary>
    /// Pakken til at frakoble sig serveren.
    /// </summary>
    public class DisconnectPacket : ClientPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectPacket"/> class.
        /// </summary>
        public DisconnectPacket()
        {
            Bytes = new byte[sizeof(byte) + sizeof(byte)];

            // PackageID
            Bytes[0] = (byte)Packets.Disconnect;
        }
    }
}