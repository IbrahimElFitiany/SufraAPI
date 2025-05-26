namespace Sufra.Exceptions
{
    public class AlreadyBlockedException : Exception
    {
        public AlreadyBlockedException() { }
        public AlreadyBlockedException(string message) : base(message) { }

        public AlreadyBlockedException(string message, Exception inner) : base(message, inner) { }
    }
}
