using System;

namespace SizeTree.Core.Services.Exceptions
{
    class GetCountOfSubDirectoriesException : Exception
    {
        public GetCountOfSubDirectoriesException()
        {
        }

        public GetCountOfSubDirectoriesException(string message) : base(message)
        {
        }

        public GetCountOfSubDirectoriesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
