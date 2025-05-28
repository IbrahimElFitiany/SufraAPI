namespace Sufra.Exceptions
{
    [Serializable]
    internal class InvalidOrderStatusTransitionException : Exception
    {
        public InvalidOrderStatusTransitionException()
        {
        }

        public InvalidOrderStatusTransitionException(string? message) : base(message)
        {
        }

        public InvalidOrderStatusTransitionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}