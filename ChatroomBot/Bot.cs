using System;
using System.Collections.Generic;
using System.Linq;
using Chatroom_Client_Backend;
using ChatroomBot.Exceptions;

#nullable enable
namespace ChatroomBot
{
    public class Bot
    {
        private readonly NetworkClient networkClient;
        private readonly Member?[] members = new Member?[byte.MaxValue];

        public Bot(string nickname, string hostname, ushort port)
        {
            networkClient = new NetworkClient(nickname + " [bot]", hostname, port);

            networkClient.onConnect += NetworkClient_onConnect;
            networkClient.onDisconnect += NetworkClient_onDisconnect;
            networkClient.onLogMessage += NetworkClient_onLogMessage;
            networkClient.onMessage += NetworkClient_onMessage;
            networkClient.onUserInfoReceived += NetworkClient_onUserInfoReceived;
            networkClient.onUserLeft += NetworkClient_onUserLeft;

            networkClient.Connect();
        }

        public void Update() => networkClient.Update();

        public Member? GetMemberByID(int userID) => members[userID];

        protected virtual void OnMemberRemove(Member member)
        {
        }

        private void NetworkClient_onUserLeft(int obj)
        {
            members[obj] = null;
        }

        protected virtual void OnMemberJoin(Member member)
        {
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

        protected virtual void OnMessage(Message message)
        {
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

        public void Send(string content)
        {
            networkClient.SendMessage(content);
        }

        private void NetworkClient_onLogMessage((string message, DateTime timeStamp) obj)
        {
        }

        private void NetworkClient_onDisconnect()
        {
        }

        private void NetworkClient_onConnect(bool obj)
        {
        }
    }
}
