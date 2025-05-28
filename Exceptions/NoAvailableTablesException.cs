namespace Sufra.Exceptions
{
    [Serializable]
    internal class NoAvailableTablesException : Exception
    {
        public NoAvailableTablesException()
        {
        }

        public NoAvailableTablesException(string? message) : base(message)
        {
        }

        public NoAvailableTablesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}