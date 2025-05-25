namespace Sufra.Exceptions
{
    public class RestaurantNotApprovedException : Exception
    {
        public RestaurantNotApprovedException() { }

        public RestaurantNotApprovedException(string message)
            : base(message) { }

        public RestaurantNotApprovedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
