using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Chatroom_Client_Backend.Packets.ClientPackets;
using System.Threading;

namespace Chatroom_Client_Backend
{
	public class NetworkClient
	{
		TcpClient client;
		string nickName;
		NetworkStream stream;
		public ClientEvents events;

		public NetworkClient(string Nickname)
		{
			nickName = Nickname;

		}

		public bool TryConnect(string server, int port)
		{
			client = new TcpClient();

			client.BeginConnect(server, port, new AsyncCallback(delegate (IAsyncResult ar)
			{
				try
				{
					client.EndConnect(ar);

					stream = client.GetStream();
					events = new ClientEvents();
				}
				catch (SocketException e)
				{
					throw e;
				}
			}), null);


			return true;
		}

		public void Update()
		{
			//List<byte> data = new List<byte>();
			while (client.Available > 0)
			{
				//data.Add((byte)stream.ReadByte());
				bool privateMessage;
				int userID;
				byte[] unixTimeStampArray;
				byte[] messageLengthArray;
				byte[] messageArray;
				long unixTimeStamp;
				ushort messageLength;
				string message;
				byte nameLength;
				byte[] nameArray;
				string name;

				switch (stream.ReadByte())
				{
					case 1:
						// Klienten bliver pinget og vi ignorer det, tror jeg?
						break;
					case 3:
						privateMessage = stream.ReadByte() != 0;

						userID = stream.ReadByte();

						unixTimeStampArray = new byte[sizeof(long)];
						stream.Read(unixTimeStampArray, 0, sizeof(long));
						unixTimeStamp = BitConverter.ToInt64(unixTimeStampArray, 0);

						messageLengthArray = new byte[sizeof(ushort)];
						stream.Read(messageLengthArray, 0, sizeof(ushort));
						messageLength  = BitConverter.ToUInt16(messageLengthArray, 0);

						messageArray = new byte[messageLength];
						stream.Read(messageArray, 0, messageLength);
						message = BitConverter.ToString(messageArray, 0);

						events.MessageReceived(userID, message, unixTimeStamp);
						break;
					case 5:
						unixTimeStampArray = new byte[sizeof(long)];
						stream.Read(unixTimeStampArray, 0, sizeof(long));
						unixTimeStamp = BitConverter.ToInt64(unixTimeStampArray, 0);

						messageLengthArray = new byte[sizeof(ushort)];
						stream.Read(messageLengthArray, 0, sizeof(ushort));
						messageLength = BitConverter.ToUInt16(messageLengthArray, 0);

						messageArray = new byte[messageLength];
						stream.Read(messageArray, 0, messageLength);
						message = BitConverter.ToString(messageArray, 0);

						events.LogMessageReceived(message, unixTimeStamp);
						break;
					case 7:
						userID = stream.ReadByte();

						nameLength = (byte)stream.ReadByte();

						nameArray = new byte[nameLength];
						stream.Read(nameArray, 0, nameLength);
						name = BitConverter.ToString(nameArray, 0);

						events.UserInfoReceived(userID, name);
						break;
					case 9:
						//Handshake
						userID = stream.ReadByte();

						SendPacket(new TellNamePacket(nickName));
						events.UserIDReceived(userID);
						break;
					case 11:
						userID = stream.ReadByte();
						events.UserLeft(userID);
						break;
					default:
						break;
				}
			}

			/*byte[] dataArray = data.ToArray();

			switch (data[0])
			{
				case 1:
					// Klienten bliver pinget og vi ignorer det, tror jeg?
					break;
				case 3:
					events.MessageReceived(dataArray[1], BitConverter.ToString(dataArray, dataArray[12], BitConverter.ToInt32(dataArray, 10)), BitConverter.ToInt64(dataArray, 2));
					break;
				case 5:
					events.LogMessageReceived(BitConverter.ToString(dataArray, sizeof(byte) + sizeof(ushort) + sizeof(long), Convert.ToUInt16(dataArray[sizeof(byte) + sizeof(long)])), BitConverter.ToInt64(dataArray, 2));
					break;
				case 7:
					events.UserInfoReceived(dataArray[1], BitConverter.ToString(dataArray, 3, dataArray[2]));
					break;
				case 9:
					//Handshake
					events.UserIDReceived(dataArray[1]);
					SendPacket(new TellNamePacket(nickName));
					break;
				case 11:
					events.UserLeft(BitConverter.ToInt32(dataArray, 1));
					break;
				default:
					break;
			}*/
		}

		public void SendPacket(ClientPacket packet)
		{
			NetworkStream stream = client.GetStream();
			
			stream.Write(packet.bytes, 0, packet.bytes.Length);
		}
	}
}