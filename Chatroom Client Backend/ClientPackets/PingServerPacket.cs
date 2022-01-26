using static Chatroom_Client_Backend.NetworkClient;

namespace Chatroom_Client_Backend.ClientPackets
{
    /// <summary>
    /// Pakken til at ping serveren.
    /// </summary>
    public class PingServerPacket : ClientPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingServerPacket"/> class.
        /// </summary>
        public PingServerPacket()
        {
            Bytes = new byte[sizeof(byte) + sizeof(byte)];

            // PackageID
            Bytes[0] = (byte)Packets.Ping;
        }
    }
}