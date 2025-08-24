using System.Net;

namespace Sufra.Exceptions.Restaurant
{
    public class RestaurantAlreadyBlockedException : DomainException
    {
        public RestaurantAlreadyBlockedException() : base("Restaurant is already blocked.", HttpStatusCode.BadRequest) { }
        public RestaurantAlreadyBlockedException(string message) : base(message, HttpStatusCode.BadRequest) { }
    }
}
