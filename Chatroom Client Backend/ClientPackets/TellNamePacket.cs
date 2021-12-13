using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client_Backend.ClientPackets
{
	public class TellNamePacket : ClientPacket
	{
		public TellNamePacket(string nickName)
		{
			bytes = new byte[sizeof(byte) + sizeof(byte) + Encoding.UTF8.GetByteCount(nickName)];

			//PackageID
			bytes[0] = (byte)4;

			//MessageLength
			bytes[1] = (byte)Encoding.UTF8.GetByteCount(nickName);

			//Message
			byte[] nickNameBytes = Encoding.UTF8.GetBytes(nickName);

			for (int i = 0; i < nickNameBytes.Length; i++)
			{
				bytes[2 + i] =  nickNameBytes[i];
			}
		}
	}
}