namespace Sufra.Common.Types
{
    public class LoginResult<T>
    {
        public T LoginResDTO { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
