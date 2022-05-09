using System;

namespace Freshness.Common.CustomExceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message)
        {

        }
    }
}
