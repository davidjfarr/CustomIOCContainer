using System;
using System.Runtime.Serialization;

namespace IOCContainer.CustomException
{
    public class NotRegisteredException : Exception
    {
        public NotRegisteredException()
        {
        }

        public NotRegisteredException(string message)
            : base(message)
        {
        }

        public NotRegisteredException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NotRegisteredException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
