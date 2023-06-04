using System;

namespace Chatroom.NetworkClient.Bot
{
    /// <summary>
    /// Stores information regarding a chatmessage.
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// The plaintext content of the message.
        /// </summary>
        public readonly string Content;

        /// <summary>
        /// The author of the message.
        /// </summary>
        public readonly Member Author;

        /// <summary>
        /// The timestamp of when the message was created.
        /// </summary>
        public readonly DateTime CreatedAt;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> struct.
        /// </summary>
        /// <param name="content">Plaintext message content.</param>
        /// <param name="author">Author of the message.</param>
        /// <param name="createdAtLocalTime">The timestamp of message creation.</param>
        public Message(string content, Member author, DateTime createdAtLocalTime)
        {
            Content = content;
            Author = author;
            CreatedAt = createdAtLocalTime;
        }
    }
}
