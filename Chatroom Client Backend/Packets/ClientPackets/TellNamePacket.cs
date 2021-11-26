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
			//PackageID
			bytes[0] = (byte)2;

			//MessageLength
			byte[] nickNameLengthBytes = BitConverter.GetBytes((ushort)nickName.Length);

			for (int i = 0; i < nickNameLengthBytes.Length; i++)
			{
				bytes[1 + i] = nickNameLengthBytes[i];
			}
			
			//Message
			for (int i = 0; i < nickName.Length; i++)
			{
				bytes[2 + i] = Convert.ToByte(nickName[i]);
			}
		}
	}
}