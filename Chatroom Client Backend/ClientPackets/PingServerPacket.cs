using static Chatroom_Client_Backend.NetworkClient;

namespace Chatroom_Client_Backend.ClientPackets
{
    public class PingServerPacket : ClientPacket
    {
        public PingServerPacket()
        {
            bytes = new byte[sizeof(byte) + sizeof(byte)];

            //PackageID
            bytes[0] = (byte)Packets.Ping;
        }
    }
}