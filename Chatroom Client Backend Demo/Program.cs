using System;
using Chatroom_Client_Backend;

namespace Chatroom_Client_Backend_Demo
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            bool running = true;
            string nickName = "Kresten";

            // Starter klienten
            NetworkClient client = new NetworkClient(nickName, "127.0.0.1", 25565);

            client.Connect();
            while (running)
            {
                client.Update();

                string input = Console.ReadLine();

                // Gammelt
                switch (input.Split(' ')?[0])
                {
                    case "msg":
                        client.SendMessage(input);
                        // client.SendPacket(new SendMessagePacket("Hej kresten!"));
                        break;
                    case "disconnect":
                        client.Disconnect();
                        // client.SendPacket(new TellNamePacket(nickName));
                        break;
                    case "name":
                        client.ChangeName(input);
                        // client.SendPacket(new DisconnectPacket());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}