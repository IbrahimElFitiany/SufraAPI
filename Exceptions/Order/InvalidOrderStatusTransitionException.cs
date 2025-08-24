using System.Net;

namespace Sufra.Exceptions.Order
{
    internal class InvalidOrderStatusTransitionException : DomainException
    {
        public InvalidOrderStatusTransitionException() : base("Invalid order status transition.", HttpStatusCode.Conflict) { }
        public InvalidOrderStatusTransitionException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}
