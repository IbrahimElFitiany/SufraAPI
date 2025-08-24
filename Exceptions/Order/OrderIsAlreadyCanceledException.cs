using System.Net;

namespace Sufra.Exceptions.Order
{
    internal class OrderIsAlreadyCanceledException : DomainException
    {
        public OrderIsAlreadyCanceledException() : base("Order is already canceled.", HttpStatusCode.Conflict) { }
        public OrderIsAlreadyCanceledException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}
