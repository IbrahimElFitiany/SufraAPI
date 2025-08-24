using System.Net;

namespace Sufra.Exceptions.User
{
    public class PhoneAlreadyInUseException : DomainException
    {
        public PhoneAlreadyInUseException(): base("The provided phone number is already in use.", HttpStatusCode.Conflict) { }
        public PhoneAlreadyInUseException(string message): base(message, HttpStatusCode.Conflict) { }
    }
}
