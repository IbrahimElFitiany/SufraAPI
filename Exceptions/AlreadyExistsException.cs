using System.Net;

namespace Sufra.Exceptions
{
    public class AlreadyExistsException<T> : DomainException
    {
        public AlreadyExistsException() : base($"{typeof(T).Name} already exists.", HttpStatusCode.Conflict) { }
        public AlreadyExistsException(string message) : base(message, HttpStatusCode.Conflict) { }

    }
}
