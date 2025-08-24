using System.Net;

namespace Sufra.Exceptions.User
{
    internal class CustomerAlreadyReviewedException : DomainException
    {
        public CustomerAlreadyReviewedException() : base("Customer has already reviewed this item.", HttpStatusCode.Conflict) { }
        public CustomerAlreadyReviewedException(string message): base(message, HttpStatusCode.Conflict) { }
    }
}
