using System.Net;

namespace Sufra.Exceptions.Restaurant
{
    internal class NoAvailableTablesException : DomainException
    {
        public NoAvailableTablesException() : base("No available tables.", HttpStatusCode.Conflict) { }
        public NoAvailableTablesException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}
