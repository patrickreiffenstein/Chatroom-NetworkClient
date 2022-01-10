using ChatroomBot;

namespace ChatroomBotDemos
{
    class SimpleBot : Bot
    {
        public SimpleBot(string hostname, ushort port) : base("Simpletron", hostname, port)
        {
        }

        protected override void OnMessage(Message message)
        {
            if (message.Author.IsSelf)
            {
                return;
            }

            if (message.Content.StartsWith("deldet"))
            {
                Send("let Johnny = del det!");
            }
        }
    }
}
