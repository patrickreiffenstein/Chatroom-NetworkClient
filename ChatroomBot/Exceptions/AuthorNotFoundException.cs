using System;

namespace ChatroomBot.Exceptions
{
    // VS Studio Exception template
    [Serializable]
    public class AuthorNotFoundException : Exception
    {
        public AuthorNotFoundException() { }
        public AuthorNotFoundException(string message) : base(message) { }
        public AuthorNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected AuthorNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
