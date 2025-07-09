namespace Sufra.Exceptions.Auth
{
    [Serializable]
    public class RevokedTokenException : Exception
    {
        public RevokedTokenException():base("token is revoked")
        {
        }

        public RevokedTokenException(string? message) : base(message)
        {
        }

        public RevokedTokenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}