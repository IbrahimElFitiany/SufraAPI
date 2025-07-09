namespace Sufra.Exceptions.Auth
{
    [Serializable]
    public class ExpiredTokenException : Exception
    {
        public ExpiredTokenException():base("Token is expired")
        {
        }

        public ExpiredTokenException(string? message) : base(message)
        {
        }

        public ExpiredTokenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}