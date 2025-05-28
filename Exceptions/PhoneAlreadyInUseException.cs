namespace Sufra.Exceptions
{
    [Serializable]
    internal class PhoneAlreadyInUseException : Exception
    {
        public PhoneAlreadyInUseException()
        {
        }

        public PhoneAlreadyInUseException(string? message) : base(message)
        {
        }

        public PhoneAlreadyInUseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}