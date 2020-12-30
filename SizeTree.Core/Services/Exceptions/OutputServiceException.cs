using System;

namespace SizeTree.Core.Services.Exceptions
{
    public class OutputServiceException : Exception
    {
        public OutputServiceException()
        {
        }

        public OutputServiceException(string message) : base(message)
        {
        }

        public OutputServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
