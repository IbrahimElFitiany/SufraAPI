namespace Sufra.Exceptions
{
    [Serializable]
    internal class CartIsEmptyException : Exception
    {
        public CartIsEmptyException()
        {
        }

        public CartIsEmptyException(string? message) : base(message)
        {
        }

        public CartIsEmptyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}