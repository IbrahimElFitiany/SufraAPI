namespace Sufra.Exceptions
{
    public class AlreadyApprovedException : Exception
    {
        public AlreadyApprovedException() { }
        public AlreadyApprovedException(string message) : base(message) { }

        public AlreadyApprovedException(string message, Exception inner) : base(message, inner) { }
    }
}
