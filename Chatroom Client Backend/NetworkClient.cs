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
		//Variabler
		private TcpClient client;
		private string nickName;
		private NetworkStream stream;
		public bool isConnected = false;

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

		/// <summary>
		/// NetworkClient er instansen som man laver når man vil starte sit backend modul, 
		/// det er den som man bruger til at have en samtale kørende med serveren.
		/// </summary>
		/// <param name="Nickname">Ens eget navn</param>
		/// <param name="server">IP til den server man vil tilkoble sig</param>
		/// <param name="port">Porten til den server man vil tilkoble sig</param>
		public NetworkClient(string Nickname, string server, int port)
		{
			nickName = Nickname;
			Connect(server, port);
		}

		/// <summary>
		/// Connect bliver brugt til at man vælger hvilken server man vil connect sin
		/// </summary>
		/// <param name="server"></param>
		/// <param name="port"></param>
		private void Connect(string server, int port)
		{
			client = new TcpClient();

			client.BeginConnect(server, port, new AsyncCallback(delegate (IAsyncResult ar)
			{
				try
				{
					client.EndConnect(ar);

					stream = client.GetStream();
					isConnected = true;
				}
				catch (SocketException)
				{
					isConnected = false;
				}
				
			}), null);
		}

		private enum Packets
		{
			Ping = 1,
			ReceiveMessage = 3,
			LogMessage = 5,
			SendUserInfo = 7,
			SendUserID = 9,
			UserLeft = 11
		}

		/// <summary>
		/// Update er en metode som skal køres hver frame eller hvor ofte man ønsker at få opdateret information fra serveren
		/// </summary>
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

                if (stream is null)
                {
					return;
                }

				switch (stream.ReadByte())
				{
					case (byte)Packets.Ping:
						//Klienten bliver pinget og vi ignorer det
						break;
					case (byte)Packets.ReceiveMessage:
						privateMessage = stream.ReadByte() != 0;

						userID = stream.ReadByte();

						unixTimeStampArray = new byte[sizeof(long)];
						stream.Read(unixTimeStampArray, 0, sizeof(long));

						DateTimeOffset unixTime = DateTimeOffset.FromUnixTimeSeconds(BitConverter.ToInt64(unixTimeStampArray, 0) / 1000);
						unixTimeStamp = unixTime.ToLocalTime().DateTime;

						messageLengthArray = new byte[sizeof(ushort)];
						stream.Read(messageLengthArray, 0, sizeof(ushort));
						messageLength  = BitConverter.ToUInt16(messageLengthArray, 0);

						messageArray = new byte[messageLength];
						stream.Read(messageArray, 0, messageLength);
						message = Encoding.UTF8.GetString(messageArray);

						onMessage?.Invoke((userID, message, unixTimeStamp));
						break;
					case (byte)Packets.LogMessage:
						unixTimeStampArray = new byte[sizeof(long)];
						stream.Read(unixTimeStampArray, 0, sizeof(long));

						DateTimeOffset unixTime2 = DateTimeOffset.FromUnixTimeSeconds(BitConverter.ToInt64(unixTimeStampArray, 0) / 1000);
						unixTimeStamp = unixTime2.ToLocalTime().DateTime;

						messageLengthArray = new byte[sizeof(ushort)];
						stream.Read(messageLengthArray, 0, sizeof(ushort));
						messageLength = BitConverter.ToUInt16(messageLengthArray, 0);

						messageArray = new byte[messageLength];
						stream.Read(messageArray, 0, messageLength);
						message = Encoding.UTF8.GetString(messageArray);

						onLogMessage?.Invoke((message, unixTimeStamp));
						break;
					case (byte)Packets.SendUserInfo:
						userID = stream.ReadByte();

						nameLength = (byte)stream.ReadByte();

						nameArray = new byte[nameLength];
						stream.Read(nameArray, 0, nameLength);
						name = Encoding.UTF8.GetString(nameArray);

						onUserInfoReceived?.Invoke((userID, name));
						break;
					case (byte)Packets.SendUserID:
						//Handshake
						userID = stream.ReadByte();

						SendPacket(new TellNamePacket(nickName));
						onUserIDReceived?.Invoke(userID);
						break;
					case (byte)Packets.UserLeft:
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
            try
            {
				NetworkStream stream = client.GetStream();
				stream.Write(packet.bytes, 0, packet.bytes.Length);
			}
			catch (Exception)
            {
				return;
            }	
		}

		/// <summary>
		/// Metode til at sende beskeder i chatrummet
		/// </summary>
		/// <param name="message">Beskeden man ønsker at sende</param>
		public void SendMessage(string message)
		{
			SendPacket(new SendMessagePacket(message));
		}

		/// <summary>
		/// Metoden til at skifte sit navn
		/// </summary>
		/// <param name="nickName">Det navn man ønsker at skifte til</param>
		public void ChangeName(string nickName)
		{
			SendPacket(new TellNamePacket(nickName));
		}

		/// <summary>
		/// Metoden til at frakoble sig serveren
		/// </summary>
		public void Disconnect()
		{
			SendPacket(new DisconnectPacket());
		}
	}
}