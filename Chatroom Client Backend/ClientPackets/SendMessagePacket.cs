using System;
using System.Text;
using static Chatroom_Client_Backend.NetworkClient;

namespace Chatroom_Client_Backend.ClientPackets
{
	public class SendMessagePacket : ClientPacket
	{
		public SendMessagePacket(string message)
		{
			bytes = new byte[sizeof(byte) + sizeof(byte) + sizeof(ushort) + Encoding.UTF8.GetByteCount(message)];

			//PackageID
			bytes[0] = (byte)Packets.SendMessage;

			//UserID
			bytes[1] = (byte)0;

			//MessageLength
			byte[] messageLengthBytes = BitConverter.GetBytes((ushort)Encoding.UTF8.GetByteCount(message));

			for (int i = 0; i < messageLengthBytes.Length; i++)
			{
				bytes[2 + i] = messageLengthBytes[i];
			}

			//Message
			byte[] messageBytes = Encoding.UTF8.GetBytes(message);

			for (int i = 0; i < messageBytes.Length; i++)
			{
				bytes[4 + i] = messageBytes[i];
			}
		}
	}
}