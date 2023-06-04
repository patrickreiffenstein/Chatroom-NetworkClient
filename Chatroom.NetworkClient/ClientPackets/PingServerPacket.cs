using static Chatroom.NetworkClient.NetworkClient;

namespace Chatroom.NetworkClient.ClientPackets
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