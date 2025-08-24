using System.Net;

namespace Sufra.Exceptions.Restaurant
{
    internal class OutOfOpeningHoursException : DomainException
    {
        public OutOfOpeningHoursException() : base("Action not allowed outside opening hours.", HttpStatusCode.Conflict) { }
        public OutOfOpeningHoursException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}