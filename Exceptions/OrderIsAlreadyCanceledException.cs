namespace Sufra.Exceptions
{
    [Serializable]
    internal class OrderIsAlreadyCanceledException : Exception
    {
        public OrderIsAlreadyCanceledException()
        {
        }

        public OrderIsAlreadyCanceledException(string? message) : base(message)
        {
        }

        public OrderIsAlreadyCanceledException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}