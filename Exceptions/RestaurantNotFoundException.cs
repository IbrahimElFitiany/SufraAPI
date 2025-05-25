namespace Sufra.Exceptions
{
    public class RestaurantNotFoundException : Exception
    {
        public RestaurantNotFoundException() {}
        public RestaurantNotFoundException(string message): base(message) { }

        public RestaurantNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
