using System;

namespace SizeTree.Core.Services.Exceptions
{
    class CalculateFolderSizesAsyncStreamException : Exception
    {
        public CalculateFolderSizesAsyncStreamException()
        {
        }

        public CalculateFolderSizesAsyncStreamException(string message) : base(message)
        {
        }

        public CalculateFolderSizesAsyncStreamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
