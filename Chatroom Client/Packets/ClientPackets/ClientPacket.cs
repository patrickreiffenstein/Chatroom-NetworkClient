using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client.Packets.ClientPackets
{
	public enum ClientPacketType : byte
	{
		SendMesage = 2,
		ChangeName = 4
	}

	public abstract class ClientPacket
	{
		public ClientPacketType clientPacket { get; protected set; }

		public abstract byte[] Serialize();
	}
}
