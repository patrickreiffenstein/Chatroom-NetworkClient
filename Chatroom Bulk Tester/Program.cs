using System;
using System.Collections.Generic;
using Chatroom_Client_Backend;
using Sharprompt;

namespace Chatroom_Bulk_Tester
{
    class Program
    {
		public enum MainCommand
        {
			TilføjBrugere,
            Luk
        }

		static List<ServerUserInstance> serverUsers = new List<ServerUserInstance>();

        static void Main(string[] args)
        {
			string serverIP = Prompt.Input<string>("Indtast server IP", "127.0.0.1");
			int serverPort = Prompt.Input<int>("Indtast server port", 25565);
			string username = Prompt.Input<string>("Indtast brugernavn", "Anonym bot");

            while (true)
            {
				MainCommand mainCommand = Prompt.Select<MainCommand>("Vælg en indstilling");

                switch (mainCommand)
                {
                    case MainCommand.TilføjBrugere:
						int clientCount = Prompt.Input<int>("Hvor mange?");

                        List<ServerUserInstance> newUserInstances = new List<ServerUserInstance>(clientCount);
                        for (int i = 0; i < clientCount; i++)
                        {
                            var userInstance = new ServerUserInstance(serverIP, serverPort, username, serverUsers.Count);

                            serverUsers.Add(userInstance);
                            newUserInstances.Add(userInstance);
						}

                        // Refresh newly added clients to perform handshake.
                        foreach (var item in newUserInstances)
                        {
                            item.Update();
                        }

                        // Refresh old ones as well.
                        foreach (var item in serverUsers)
                        {
                            item.Update();
                        }
						continue;
                    case MainCommand.Luk:
                        foreach (var item in serverUsers)
                        {
                            item.Disconnect();
                        }
                        return;
                    default:
                        break;
                }
            }
		}

		class ServerUserInstance
        {
			readonly NetworkClient networkClient;
            readonly int ID;

			public ServerUserInstance(string serverIP, int serverPort, string username, int ID)
            {
                this.ID = ID;
				networkClient = new NetworkClient(username, serverIP, serverPort);
                networkClient.onConnect += NetworkClient_onConnect;
                networkClient.onDisconnect += NetworkClient_onDisconnect;
                networkClient.Connect();
            }

            public void Disconnect() => networkClient?.Disconnect();

            public void Update() => networkClient?.Update();

            private void NetworkClient_onDisconnect()
            {
                Console.WriteLine($"[{ID}] Frakoblet");
            }

            private void NetworkClient_onConnect(bool obj)
            {
                Console.WriteLine($"[{ID}] {(obj ? "Forbandt til server" : "Kunne ikke forbinde")}");
            }
        }
    }
}
