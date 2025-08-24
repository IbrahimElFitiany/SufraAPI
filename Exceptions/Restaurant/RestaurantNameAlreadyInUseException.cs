using System.Net;

namespace Sufra.Exceptions.Restaurant
{
    internal class RestaurantNameAlreadyInUseException : DomainException
    {
        public RestaurantNameAlreadyInUseException() : base("Restaurant name is already in use.", HttpStatusCode.BadRequest) { }
        public RestaurantNameAlreadyInUseException(string message) : base(message, HttpStatusCode.BadRequest) { }
    }
}
