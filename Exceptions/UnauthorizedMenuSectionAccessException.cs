namespace Sufra.Exceptions
{
    public class UnauthorizedMenuSectionAccessException:Exception
    {
        public UnauthorizedMenuSectionAccessException() {}
        public UnauthorizedMenuSectionAccessException(string message): base(message) {}
        public UnauthorizedMenuSectionAccessException(string message, Exception inner):base(message, inner) { }

    }
}
