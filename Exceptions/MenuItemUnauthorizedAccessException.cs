namespace Sufra.Exceptions
{
    [Serializable]
    internal class MenuItemUnauthorizedAccessException : Exception
    {
        public MenuItemUnauthorizedAccessException()
        {
        }

        public MenuItemUnauthorizedAccessException(string? message) : base(message)
        {
        }

        public MenuItemUnauthorizedAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}