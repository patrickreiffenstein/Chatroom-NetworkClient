using System;
using Chatroom_Client_Backend;
using ChatroomBot.Exceptions;

#nullable enable
namespace ChatroomBot
{
    /// <summary>
    /// The base class for implementing bots.
    /// </summary>
    public class Bot
    {
        private readonly NetworkClient networkClient;
        private readonly Member?[] members = new Member?[byte.MaxValue];

        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="nickname">The nickname the of the bot.</param>
        /// <param name="hostname">The IP or hostname of the chatserver.</param>
        /// <param name="port">The port of the chatserver.</param>
        public Bot(string nickname, string hostname, ushort port)
        {
            networkClient = new NetworkClient(nickname + " [bot]", hostname, port);

            networkClient.OnConnect += NetworkClient_onConnect;
            networkClient.OnDisconnect += NetworkClient_onDisconnect;
            networkClient.OnLogMessage += NetworkClient_onLogMessage;
            networkClient.OnMessage += NetworkClient_onMessage;
            networkClient.OnUserInfoReceived += NetworkClient_onUserInfoReceived;
            networkClient.OnUserLeft += NetworkClient_onUserLeft;

            networkClient.Connect();
        }

        /// <summary>
        /// Should be called at regular intervals to handle networking.
        /// </summary>
        public void Update() => networkClient.Update();

        /// <summary>
        /// Looksup member by their ID.
        /// Returns null, if member with that ID doesn't exist.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>The member instance, if found, otherwise null.</returns>
        public Member? GetMemberByID(int userID) => members[userID];

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="content">The contents of the message.</param>
        public void Send(string content)
        {
            networkClient.SendMessage(content);
        }

        /// <summary>
        /// Invoked when a member has disconnected.
        /// </summary>
        /// <param name="member">The member.</param>
        protected virtual void OnMemberRemove(Member member)
        {
        }

        /// <summary>
        /// Invoked when a member has joined.
        /// </summary>
        /// <param name="member">The member.</param>
        protected virtual void OnMemberJoin(Member member)
        {
        }

        /// <summary>
        /// Invoked when a message has been received.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void OnMessage(Message message)
        {
        }

        /// <summary>
        /// Invoked when bot is disconnected.
        /// </summary>
        protected virtual void OnDisconnected()
        {
        }

        /// <summary>
        /// Invoked when connection request has finished.
        /// </summary>
        /// <param name="connected">Whether a connection was successful.</param>
        protected virtual void OnConnectionFinished(bool connected)
        {
        }

        private void NetworkClient_onUserLeft(int obj)
        {
            members[obj] = null;
        }

        private void NetworkClient_onUserInfoReceived((int userID, string name) obj)
        {
            bool newlyJoined = members[obj.userID] is null;

            members[obj.userID] = new Member((byte)obj.userID, obj.name, DateTime.UtcNow, networkClient.ClientID == obj.userID);

            if (newlyJoined)
            {
                OnMemberJoin(members[obj.userID].Value);
            }
        }

        private void NetworkClient_onMessage((int user, string message, DateTime timeStamp) obj)
        {
            Member? author = GetMemberByID(obj.user);
            if (author is null)
            {
                throw new AuthorNotFoundException();
            }

            OnMessage(new Message(obj.message, author.Value, obj.timeStamp));
        }

        private void NetworkClient_onLogMessage((string message, DateTime timeStamp) obj)
        {
            OnMessage(new Message(obj.message, Member.Server, obj.timeStamp));
        }

        private void NetworkClient_onDisconnect()
        {
            OnDisconnected();
        }

        private void NetworkClient_onConnect(bool obj)
        {
            OnConnectionFinished(obj);
        }
    }
}
