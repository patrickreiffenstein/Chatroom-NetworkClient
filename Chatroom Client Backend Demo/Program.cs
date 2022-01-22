using System;
using Chatroom_Client_Backend;

namespace Chatroom_Client_Backend_Demo
{
    /// <summary>
    /// Program-klassen til demoen.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main metoden som starter med at blive kørt.
        /// </summary>
        /// <param name="args">Parametre som programmet bruger.</param>
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
                        break;
                    case "disconnect":
                        client.Disconnect();
                        break;
                    case "name":
                        client.ChangeName(input);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}