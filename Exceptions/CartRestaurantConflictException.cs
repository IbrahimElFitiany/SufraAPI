namespace Sufra.Exceptions
{
    [Serializable]
    internal class CartRestaurantConflictException : Exception
    {
        public CartRestaurantConflictException()
        {
        }

        public CartRestaurantConflictException(string? message) : base(message)
        {
        }

        public CartRestaurantConflictException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}