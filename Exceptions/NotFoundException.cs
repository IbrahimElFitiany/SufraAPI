using System.Net;

namespace Sufra.Exceptions
{
    internal class NotFoundException<T> : DomainException
    {
        public NotFoundException() : base($"{typeof(T).Name} not found.", HttpStatusCode.NotFound) { }
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound) { }
    }
}
