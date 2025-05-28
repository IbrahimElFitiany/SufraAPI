namespace Sufra.Exceptions
{
    [Serializable]
    internal class CartNotFoundException : Exception
    {
        public CartNotFoundException()
        {
        }

        public CartNotFoundException(string? message) : base(message)
        {
        }

        public CartNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}