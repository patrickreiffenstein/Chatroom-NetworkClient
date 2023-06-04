using System;
using System.Net.Sockets;
using System.Text;
using Chatroom.NetworkClient.ClientPackets;

namespace Chatroom.NetworkClient
{
    /// <summary>
    /// Klassen til netværks klienten.
    /// Dette er det backend man bruger i selve klienten for at tilgå servere.
    /// </summary>
    public class NetworkClient
    {
        // Variabler
        private TcpClient client;
        private string nickName;
        private NetworkStream stream;
        private string server;
        private int port;
        private bool isTimedOut = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkClient"/> class.
        /// NetworkClient er instansen som man laver når man vil starte sit backend modul,
        /// det er den som man bruger til at have en samtale kørende med serveren.
        /// </summary>
        /// <param name="nickname">Ens eget navn.</param>
        /// <param name="server">IP til den server man vil tilkoble sig.</param>
        /// <param name="port">Porten til den server man vil tilkoble sig.</param>
        public NetworkClient(string nickname, string server, int port)
        {
            nickName = nickname;
            this.server = server;
            this.port = port;
        }

        /// <summary>
        /// Pakke nummer 3
        /// </summary>
        public event Action<(int user, string message, DateTime timeStamp)> OnMessage;

        /// <summary>
        /// Pakke nummer 5
        /// </summary>
        public event Action<(string message, DateTime timeStamp)> OnLogMessage;

        /// <summary>
        /// Pakke nummer 7
        /// </summary>
        public event Action<(int userID, string name)> OnUserInfoReceived;

        /// <summary>
        /// Returnere en boolks værdi alt efter om den er lykkedes (true) eller fejlet (false), i at tilkoble sig til serveren
        /// </summary>
        public event Action<bool> OnConnect;

        /// <summary>
        /// Pakke nummer 11
        /// </summary>
        public event Action<int> OnUserLeft;

        /// <summary>
        /// Pakke nummer 10
        /// </summary>
        public event Action OnDisconnect;

        /// <summary>
        /// En enum som holder på alle ID'er af de forskellige packets.
        /// </summary>
        public enum Packets
        {
            /// <summary>
            /// Ping packet ID
            /// </summary>
            Ping = 1,

            /// <summary>
            /// Send message packet ID
            /// </summary>
            SendMessage = 2,

            /// <summary>
            /// Receive message packet ID
            /// </summary>
            ReceiveMessage = 3,

            /// <summary>
            /// Tell name packet ID
            /// </summary>
            TellName = 4,

            /// <summary>
            /// Log message packet ID
            /// </summary>
            LogMessage = 5,

            /// <summary>
            /// Send user info packet ID
            /// </summary>
            SendUserInfo = 7,

            /// <summary>
            /// Send user ID packet ID
            /// </summary>
            SendUserID = 9,

            /// <summary>
            /// Disconnect packet ID
            /// </summary>
            Disconnect = 10,

            /// <summary>
            /// User left packet ID
            /// </summary>
            UserLeft = 11,
        }

        /// <summary>
        /// Skaffer ID'et for denne klient.
        /// </summary>
        public int ClientID { get; private set; }

        /// <summary>
        /// Connect bliver brugt til at man vælger hvilken server man vil connect sin.
        /// </summary>
        public void Connect()
        {
            client = new TcpClient();

            client.BeginConnect(server, port, new AsyncCallback(delegate(IAsyncResult ar)
            {
                try
                {
                    client.EndConnect(ar);

                    stream = client.GetStream();
                }
                catch (SocketException)
                {
                    OnConnect(false);
                }
            }), null);
        }

        /// <summary>
        /// Update er en metode som skal køres hver frame eller hvor ofte man ønsker at få opdateret information fra serveren.
        /// </summary>
        public void Update()
        {
            if (client is null)
            {
                if (isTimedOut)
                {
                    OnDisconnect?.Invoke();
                }

                isTimedOut = true;
                return;
            }

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
                        // Klienten bliver pinget og vi ignorer det
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
                        messageLength = BitConverter.ToUInt16(messageLengthArray, 0);

                        messageArray = new byte[messageLength];
                        stream.Read(messageArray, 0, messageLength);
                        message = Encoding.UTF8.GetString(messageArray);

                        OnMessage?.Invoke((userID, message, unixTimeStamp));
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

                        OnLogMessage?.Invoke((message, unixTimeStamp));
                        break;
                    case (byte)Packets.SendUserInfo:
                        userID = stream.ReadByte();

                        nameLength = (byte)stream.ReadByte();

                        nameArray = new byte[nameLength];
                        stream.Read(nameArray, 0, nameLength);
                        name = Encoding.UTF8.GetString(nameArray);

                        OnUserInfoReceived?.Invoke((userID, name));
                        break;
                    case (byte)Packets.SendUserID:
                        // Handshake
                        ClientID = stream.ReadByte();

                        SendPacket(new TellNamePacket(nickName));
                        OnConnect?.Invoke(true);
                        break;
                    case (byte)Packets.UserLeft:
                        userID = stream.ReadByte();

                        // Hvis det nu skulle ske
                        if (userID == ClientID)
                        {
                            OnDisconnect?.Invoke();
                            break;
                        }

                        OnUserLeft?.Invoke(userID);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Metode til at sende beskeder i chatrummet.
        /// </summary>
        /// <param name="message">Beskeden man ønsker at sende.</param>
        /// <param name="id">ID på person(er) man vil sende beskeden til.</param>
        public void SendMessage(string message, byte id = 0)
        {
            SendPacket(new SendMessagePacket(message, id));
        }

        /// <summary>
        /// Metoden til at skifte sit navn.
        /// </summary>
        /// <param name="nickName">Det navn man ønsker at skifte til.</param>
        public void ChangeName(string nickName)
        {
            SendPacket(new TellNamePacket(nickName));
        }

        /// <summary>
        /// Metoden til at frakoble sig serveren.
        /// </summary>
        public void Disconnect()
        {
            SendPacket(new DisconnectPacket());
        }

        /// <summary>
        /// Metoden til at sende et ping til serveren.
        /// </summary>
        public void PingServer()
        {
            SendPacket(new PingServerPacket());
        }

        private void SendPacket(ClientPacket packet)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                stream.Write(packet.Bytes, 0, packet.Bytes.Length);
            }
            catch (Exception)
            {
                OnDisconnect?.Invoke();
            }
        }
    }
}