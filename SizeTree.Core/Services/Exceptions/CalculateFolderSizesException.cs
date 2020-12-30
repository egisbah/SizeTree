using System;

namespace SizeTree.Core.Services.Exceptions
{
    public class CalculateFolderSizesException : Exception
    {
        public CalculateFolderSizesException()
        {
        }

        public CalculateFolderSizesException(string message) : base(message)
        {
        }

        public CalculateFolderSizesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
