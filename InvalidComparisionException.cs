using System;

namespace ObjectChanged
{
    /// <summary>
    ///     InvalidComparisionException is thrown when an invalid object comparison is attempted.
    /// </summary>
    public class InvalidComparisionException : Exception
    {
        public InvalidComparisionException()
        {
        }

        public InvalidComparisionException(string message) : base(message)
        {
        }
    }
}