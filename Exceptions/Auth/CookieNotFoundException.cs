namespace Sufra.Exceptions.Auth
{
    [Serializable]
    internal class CookieNotFoundException : Exception
    {
        public CookieNotFoundException():base("Cookie Not Found")
        {
        }

        public CookieNotFoundException(string? message) : base(message)
        {
        }

        public CookieNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}