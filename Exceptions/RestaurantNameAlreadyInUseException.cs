namespace SufraMVC.Exceptions
{
    public class RestaurantNameAlreadyInUseException : Exception
    {
        public RestaurantNameAlreadyInUseException() {}
        public RestaurantNameAlreadyInUseException(string message): base(message) { }

        public RestaurantNameAlreadyInUseException(string message, Exception inner) : base(message, inner) { }
    }
}
