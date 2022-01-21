using static Chatroom_Client_Backend.NetworkClient;

namespace Chatroom_Client_Backend.ClientPackets
{
    public class DisconnectPacket : ClientPacket
    {
        public DisconnectPacket()
        {
            bytes = new byte[sizeof(byte) + sizeof(byte)];

            //PackageID
            bytes[0] = (byte)Packets.Disconnect;
        }
    }
}