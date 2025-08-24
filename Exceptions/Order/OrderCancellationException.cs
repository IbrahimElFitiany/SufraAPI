using System.Net;

namespace Sufra.Exceptions.Order
{
    internal class OrderCancellationException : DomainException
    {
        public OrderCancellationException() : base("Order cannot be cancelled.", HttpStatusCode.Conflict) { }
        public OrderCancellationException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}
