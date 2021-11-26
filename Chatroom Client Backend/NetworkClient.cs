using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Chatroom_Client_Backend.ClientPackets;
using System.Threading;

namespace Chatroom_Client_Backend
{
	public class NetworkClient
	{
		TcpClient client;
		string nickName;
		NetworkStream stream;

		///Events
		//3
		public event Action<(int user, string message, DateTime timeStamp)> onMessage;
		//5
		public event Action<(string message, DateTime timeStamp)> onLogMessage;
		//7
		public event Action<(int userID, string name)> onUserInfoReceived;
		//9
		public event Action<int> onUserIDReceived;
		//11
		public event Action<int> onUserLeft;

		public NetworkClient(string Nickname)
		{
			nickName = Nickname;
		}

		public void Connect(string server, int port)
		{
			client = new TcpClient();

			client.BeginConnect(server, port, new AsyncCallback(delegate (IAsyncResult ar)
			{
				try
				{
					client.EndConnect(ar);

					stream = client.GetStream();
				}
				catch (SocketException e)
				{
					throw e;
				}
			}), null);
		}

		public void Update()
		{
			while (client.Available > 0)
			{
				bool privateMessage;
				int userID;
				byte[] unixTimeStampArray;
				byte[] messageLengthArray;
				byte[] messageArray;
				DateTime unixTimeStamp;
				ushort messageLength;
				string message;
				byte nameLength;
				byte[] nameArray;
				string name;

				switch (stream.ReadByte())
				{
					case 1:
						// Klienten bliver pinget og vi ignorer det
						break;
					case 3:
						privateMessage = stream.ReadByte() != 0;

						userID = stream.ReadByte();

						unixTimeStampArray = new byte[sizeof(long)];
						stream.Read(unixTimeStampArray, 0, sizeof(long));
						unixTimeStamp = DateTime.FromBinary(BitConverter.ToInt64(unixTimeStampArray, 0)).ToLocalTime();

						messageLengthArray = new byte[sizeof(ushort)];
						stream.Read(messageLengthArray, 0, sizeof(ushort));
						messageLength  = BitConverter.ToUInt16(messageLengthArray, 0);

						messageArray = new byte[messageLength];
						stream.Read(messageArray, 0, messageLength);
						message = BitConverter.ToString(messageArray, 0);

						onMessage?.Invoke((userID, message, unixTimeStamp));
						break;
					case 5:
						unixTimeStampArray = new byte[sizeof(long)];
						stream.Read(unixTimeStampArray, 0, sizeof(long));
						unixTimeStamp = DateTime.FromBinary(BitConverter.ToInt64(unixTimeStampArray, 0)).ToLocalTime();

						messageLengthArray = new byte[sizeof(ushort)];
						stream.Read(messageLengthArray, 0, sizeof(ushort));
						messageLength = BitConverter.ToUInt16(messageLengthArray, 0);

						messageArray = new byte[messageLength];
						stream.Read(messageArray, 0, messageLength);
						message = BitConverter.ToString(messageArray, 0);

						onLogMessage?.Invoke((message, unixTimeStamp));
						break;
					case 7:
						userID = stream.ReadByte();

						nameLength = (byte)stream.ReadByte();

						nameArray = new byte[nameLength];
						stream.Read(nameArray, 0, nameLength);
						name = BitConverter.ToString(nameArray, 0);

						onUserInfoReceived?.Invoke((userID, name));
						break;
					case 9:
						//Handshake
						userID = stream.ReadByte();

						SendPacket(new TellNamePacket(nickName));
						onUserIDReceived?.Invoke(userID);
						break;
					case 11:
						userID = stream.ReadByte();
						onUserLeft?.Invoke(userID);
						break;
					default:
						break;
				}
			}
		}

		private void SendPacket(ClientPacket packet)
		{
			NetworkStream stream = client.GetStream();
			
			stream.Write(packet.bytes, 0, packet.bytes.Length);
		}

		public void SendMessage(string message)
		{
			SendPacket(new SendMessagePacket(message));
		}

		public void ChangeName(string nickName)
		{
			SendPacket(new TellNamePacket(nickName));
		}

		public void Disconnect()
		{
			SendPacket(new DisconnectPacket());
		}
	}
}