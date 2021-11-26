using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatroom_Client_Backend;
using Chatroom_Client_Backend.Packets.ClientPackets;

namespace Chatroom_Client_Backend_Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			bool running = true;
			string nickName = "Kresten";
			// Starter klienten
			NetworkClient client = new NetworkClient(nickName);
			bool connected = client.TryConnect("10.29.139.215", 25565);

			Console.WriteLine(connected);
			
			while (running)
			{
				string input = Console.ReadLine();
				
				switch (input)
				{
					case "2":
						client.SendPacket(new SendMessagePacket("Hej kresten!"));
						break;
					case "4":
						client.SendPacket(new TellNamePacket(nickName));
						break;
					case "10":
						client.SendPacket(new DisconnectPacket());
						break;
					default:
						break;
				}
			}
		}
	}
}
