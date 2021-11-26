using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client_Backend.Packets.ClientPackets
{
	public class TellNamePacket : ClientPacket
	{
		public TellNamePacket(string nickName)
		{
			bytes = new byte[sizeof(byte) + sizeof(ushort) + nickName.Length];

			//PackageID
			bytes[0] = (byte)4;

			//MessageLength
			byte[] nickNameLengthBytes = BitConverter.GetBytes((ushort)Encoding.UTF8.GetByteCount(nickName));

			for (int i = 0; i < nickNameLengthBytes.Length; i++)
			{
				bytes[1 + i] = nickNameLengthBytes[i];
			}

			//Message
			byte[] nickNameBytes = Encoding.UTF8.GetBytes(nickName);

			for (int i = 0; i < nickName.Length; i++)
			{
				bytes[3 + i] =  nickNameBytes[i];
			}
		}
	}
}