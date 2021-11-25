using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client.Packets.ClientPackets
{
	public class SendMessagePacket : ClientPacket
	{
		public SendMessagePacket(string message)
		{
			//PackageID
			bytes[0] = (byte)2;

			//UserID
			bytes[1] = (byte)0;

			//MessageLength
			byte[] messageLengthBytes = BitConverter.GetBytes((ushort)message.Length);

			for (int i = 0; i < messageLengthBytes.Length; i++)
			{
				bytes[2 + i] = messageLengthBytes[i];
			}
			
			//Message
			for (int i = 0; i < message.Length; i++)
			{
				bytes[3 + i] = Convert.ToByte(message[i]);
			}
		}
	}
}