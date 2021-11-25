using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom_Client.Packets.ServerPackets
{
	class ServerPacket
	{
		public enum ServerPacketType : byte
		{
			Ping = 1,
			ReceiveMessage = 3,
			LogMessage = 5,
			SendUserInfo = 7,
			SendUserID = 9,
			UserLeft = 11,
		}

		public abstract class ClientPacket
		{
			public ServerPacketType serverPacket { get; protected set; }

			public abstract void Parse(NetworkStream stream);
		}
	}
}
