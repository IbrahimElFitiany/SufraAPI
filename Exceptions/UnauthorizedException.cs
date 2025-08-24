using System.Net;

namespace Sufra.Exceptions
{
    internal class UnauthorizedException : DomainException
    {
        public UnauthorizedException() : base("Unauthorized access.", HttpStatusCode.Unauthorized) { }
        public UnauthorizedException(string message) : base(message, HttpStatusCode.Unauthorized) { }
    }
}
