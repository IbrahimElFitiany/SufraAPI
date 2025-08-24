using System.Net;

namespace Sufra.Exceptions
{
    public class DomainException:Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public DomainException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError):base(message) 
        {
            StatusCode = statusCode;
        }
    }
}
