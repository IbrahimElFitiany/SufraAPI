using System.Net;

namespace Sufra.Exceptions.Cart
{
    internal class CartRestaurantConflictException : DomainException
    {
        public CartRestaurantConflictException() : base("Cart contains items from a different restaurant.", HttpStatusCode.Conflict) { }
        public CartRestaurantConflictException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}
