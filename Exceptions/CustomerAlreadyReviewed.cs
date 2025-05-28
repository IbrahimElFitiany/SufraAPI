namespace Sufra.Exceptions
{
    [Serializable]
    internal class CustomerAlreadyReviewed : Exception
    {
        public CustomerAlreadyReviewed()
        {
        }

        public CustomerAlreadyReviewed(string? message) : base(message)
        {
        }

        public CustomerAlreadyReviewed(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}