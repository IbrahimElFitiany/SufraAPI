namespace Sufra.Exceptions
{
    [Serializable]
    internal class OrderUnauthorizedAccessException : Exception
    {
        public OrderUnauthorizedAccessException()
        {
        }

        public OrderUnauthorizedAccessException(string? message) : base(message)
        {
        }

        public OrderUnauthorizedAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}