using System;

namespace SizeTree.Core.Services.Exceptions
{
    public class FileServiceException : Exception
    {
        public FileServiceException()
        {
        }

        public FileServiceException(string message)
            : base(message)
        {
        }

        public FileServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
