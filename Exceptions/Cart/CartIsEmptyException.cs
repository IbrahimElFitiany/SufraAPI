using System.Net;

namespace Sufra.Exceptions.Cart
{
    internal class CartIsEmptyException : DomainException
    {
        public CartIsEmptyException(): base("Cart is empty.", HttpStatusCode.Conflict){}
        public CartIsEmptyException(string message): base(message, HttpStatusCode.Conflict){}
    }
}