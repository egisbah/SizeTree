using System;

namespace SizeTree.Core.Services.Exceptions
{
    public class WriteOutputToFileException : Exception
    {
        public WriteOutputToFileException()
        {
        }

        public WriteOutputToFileException(string message) : base(message)
        {
        }

        public WriteOutputToFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
