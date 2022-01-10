using System;

namespace ChatroomBot
{
    public struct Message
    {
        public readonly string Content;
        public readonly Member Author;
        public readonly DateTime CreatedAt;

        public Message(string content, Member author, DateTime createdAtLocalTime)
        {
            Content = content;
            Author = author;
            CreatedAt = createdAtLocalTime;
        }
    }
}
