using System.Threading;

namespace Chatroom.NetworkClient.Bot.Demos
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Bot testBot = new SimpleBot("10.242.79.240", 25565);

            while (true)
            {
                testBot.Update();
                Thread.Sleep(500);
            }
        }
    }
}
