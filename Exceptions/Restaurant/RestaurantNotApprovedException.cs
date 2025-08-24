using System.Net;

namespace Sufra.Exceptions.Restaurant
{
    public class RestaurantNotApprovedException : DomainException
    {
        public RestaurantNotApprovedException() : base("The restaurant is not approved." , HttpStatusCode.Forbidden) { }
        public RestaurantNotApprovedException(string message) : base(message, HttpStatusCode.Forbidden) { }
    }
}
