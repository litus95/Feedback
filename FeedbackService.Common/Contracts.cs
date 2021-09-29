using System;
using System.Runtime.Serialization;

namespace FeedbackService.Common
{
    public static class Contracts
    {
        public static void Require(bool precondition, string message = "")
        {
            if (!precondition)
                throw new ContractException(message);
        }

        [Serializable]
        public class ContractException : Exception
        {
            public ContractException()
            {
            }

            public ContractException(string message)
                : base(message)
            {
            }

            public ContractException(string message, Exception inner)
                : base(message, inner)
            {
            }

            protected ContractException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
