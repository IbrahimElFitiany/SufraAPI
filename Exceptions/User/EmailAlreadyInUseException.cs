using System.Net;

namespace Sufra.Exceptions.User
{
    public class EmailAlreadyInUseException : DomainException
    {
        public EmailAlreadyInUseException(): base("The provided email address is already in use.", HttpStatusCode.Conflict) { }
        public EmailAlreadyInUseException(string message): base(message, HttpStatusCode.Conflict) { }
    }
}
