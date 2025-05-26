namespace Sufra.Exceptions
{
    public class MenuSectionUnauthorizedAccessException:Exception
    {
        public MenuSectionUnauthorizedAccessException() {}
        public MenuSectionUnauthorizedAccessException(string message): base(message) {}
        public MenuSectionUnauthorizedAccessException(string message, Exception inner):base(message, inner) { }

    }
}
