namespace Sufra.Exceptions
{
    [Serializable]
    internal class CartItemNotFoundException : Exception
    {
        public CartItemNotFoundException()
        {
        }

        public CartItemNotFoundException(string? message) : base(message)
        {
        }

        public CartItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}