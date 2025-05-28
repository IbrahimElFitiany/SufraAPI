namespace Sufra.Exceptions
{
    [Serializable]
    internal class OutOfOpeningHoursException : Exception
    {
        public OutOfOpeningHoursException()
        {
        }

        public OutOfOpeningHoursException(string? message) : base(message)
        {
        }

        public OutOfOpeningHoursException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}