using System.Net;

namespace Sufra.Exceptions.Restaurant
{
    public class RestaurantAlreadyApprovedException : DomainException
    {
        public RestaurantAlreadyApprovedException() : base("Restaurant is already approved.", HttpStatusCode.BadRequest) { }
        public RestaurantAlreadyApprovedException(string message) : base(message, HttpStatusCode.BadRequest) { }
    }
}
