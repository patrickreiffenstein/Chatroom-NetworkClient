using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client_Backend.ClientPackets
{
	public class DisconnectPacket : ClientPacket
	{
		public DisconnectPacket()
		{
			bytes = new byte[sizeof(byte) + sizeof(byte)];

			//PackageID
			bytes[0] = (byte)2;
		}
	}
}