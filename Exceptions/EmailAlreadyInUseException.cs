namespace Sufra.Exceptions
{
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException() {}
        public EmailAlreadyInUseException(string message): base(message) { }

        public EmailAlreadyInUseException(string message, Exception inner) : base(message, inner) { }
    }
}
