using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chatroom_Client_Backend;
using Chatroom_Client_Backend.ClientPackets;

namespace Chatroom_Client_Backend_Demo
{
	class Program
	{
		public static int clientID;

		static void Main(string[] args)
		{
			bool running = true;
			string nickName = "Kresten";
			// Starter klienten
			NetworkClient client = new NetworkClient(nickName, "192.168.43.217", 25564);

			client.onUserIDReceived += onUserIDReceivedActionMethod;
			
			
			while (running)
			{
				client.Update();

				string input = Console.ReadLine();
				
				//Gammelt
				switch (input)
				{
					case "2":
						//client.SendPacket(new SendMessagePacket("Hej kresten!"));
						break;
					case "4":
						//client.SendPacket(new TellNamePacket(nickName));
						break;
					case "10":
						client.Disconnect();
						break;
					case "start":
						Main(new string[0]);
						break;
					default:
						break;
				}
			}
		}

		private static void onUserIDReceivedActionMethod(int id)
		{
			clientID = id;
		}
	}
}
