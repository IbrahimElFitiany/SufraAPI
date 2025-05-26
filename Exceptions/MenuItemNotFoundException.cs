namespace Sufra.Exceptions
{
    [Serializable]
    internal class MenuItemNotFoundException : Exception
    {
        public MenuItemNotFoundException()
        {
        }

        public MenuItemNotFoundException(string? message) : base(message)
        {
        }

        public MenuItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}