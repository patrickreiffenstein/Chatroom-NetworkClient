using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Chatroom_Client.Packets.ClientPackets;

namespace Chatroom_Client
{
	class NetworkClient
	{
		TcpClient client;

		/// <summary>
		/// Method to try to connect to the server
		/// </summary>
		/// <returns>
		/// Returns a bool about wheter or not the connection is succesful
		/// </returns>
		public bool TryConnect(string server, int port)
		{
			try
			{
				client = new TcpClient(server, port);
				ClientLoop(client);
			}
			catch (Exception e)
			{
				//Gør noget bedre med eventuelle fejl senere
				Console.WriteLine(e);
				return false;
			}
			return true;
		}

		public void ClientLoop(TcpClient client)
		{
			NetworkStream stream = client.GetStream();
			ClientEvents events = new ClientEvents();

			while (client.Connected)
			{
				List<byte> data = new List<byte>();
				while (client.Available > 0)
				{
					data[data.Count] = (byte)stream.ReadByte();
				}
				byte[] dataArray = data.ToArray();
				switch (data[0])
				{
					case 1:
						// Klienten bliver pinget og vi ignorer det, tror jeg?
						break;
					case 3:
						events.MessageReceived(data[1].ToString(), BitConverter.ToString(dataArray, dataArray[12], BitConverter.ToInt32(dataArray, 10)), BitConverter.ToInt64(dataArray, 2));
						break;
					case 5:
						events.LogMessageReceived(BitConverter.ToString(dataArray, 11, BitConverter.ToInt32(dataArray, 9)), BitConverter.ToInt64(dataArray, 2));
						break;
					case 7:
						events.UserInfoReceived(BitConverter.ToInt32(dataArray, 1), BitConverter.ToString(dataArray, 3, dataArray[2]));
						break;
					case 9:
						events.UserIDReceived(BitConverter.ToInt32(dataArray, 1));
						break;
					case 11:
						events.UserLeft(BitConverter.ToInt32(dataArray, 1));
						break;
					default:
						// Det her ville ikke være ret godt siden vi ikke kan genkende pakken.
						break;
				}
			}
		}

		public void SendPacket(ClientPacket packet)
		{
			NetworkStream stream = client.GetStream();
			

			//stream.Write(bytes, 0, bytes.Length);
		}
	}
}
