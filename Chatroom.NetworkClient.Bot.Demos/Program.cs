using System.Threading;

namespace Chatroom.NetworkClient.Bot.Demos
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Bot testBot = new SimpleBot("192.168.1.10", 25565);

            while (true)
            {
                testBot.Update();
                Thread.Sleep(500);
            }
        }
    }
}
