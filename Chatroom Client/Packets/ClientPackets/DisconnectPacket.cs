using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client.Packets.ClientPackets
{
	public class DisconnectPacket : ClientPacket
	{
		public DisconnectPacket(string message)
		{
			//PackageID
			bytes[0] = (byte)2;
		}
	}
}