using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

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
			}
			catch (Exception e)
			{
				//Gør noget bedre med eventuelle fejl senere
				Console.WriteLine(e);
				return false;
			}
			return true;
		}

		public void SendPacket(byte[] bytes)
		{
			NetworkStream stream = client.GetStream();

			stream.Write(bytes, 0, bytes.Length);
		}
	}
}
