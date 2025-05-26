namespace Sufra.Exceptions
{
    public class MenuItemAlreadyExistsException : Exception
    {
        public MenuItemAlreadyExistsException() {}
        public MenuItemAlreadyExistsException(string message): base(message) {}
        public MenuItemAlreadyExistsException(string message, Exception inner):base(message, inner) { }

    }
}
