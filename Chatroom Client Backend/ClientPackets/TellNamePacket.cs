using System.Text;
using static Chatroom_Client_Backend.NetworkClient;

namespace Chatroom_Client_Backend.ClientPackets
{
    /// <summary>
    /// Pakken til at fortælle hvad ens navn er til serveren.
    /// </summary>
    public class TellNamePacket : ClientPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TellNamePacket"/> class.
        /// </summary>
        /// <param name="nickName"> Ens nye navn. </param>
        public TellNamePacket(string nickName)
        {
            Bytes = new byte[sizeof(byte) + sizeof(byte) + Encoding.UTF8.GetByteCount(nickName)];

            // PackageID
            Bytes[0] = (byte)Packets.TellName;

            // MessageLength
            Bytes[1] = (byte)Encoding.UTF8.GetByteCount(nickName);

            // Message
            byte[] nickNameBytes = Encoding.UTF8.GetBytes(nickName);

            for (int i = 0; i < nickNameBytes.Length; i++)
            {
                Bytes[2 + i] = nickNameBytes[i];
            }
        }
    }
}