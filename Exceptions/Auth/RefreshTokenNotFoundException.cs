namespace Sufra.Exceptions.Auth
{
    [Serializable]
    public class RefreshTokenNotFoundException : Exception
    {
        public RefreshTokenNotFoundException():base("Refresh token was not found.")
        {
        }

        public RefreshTokenNotFoundException(string? message) : base(message)
        {
        }

        public RefreshTokenNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}