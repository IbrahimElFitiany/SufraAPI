namespace Sufra_MVC.Exceptions
{
    public class MenuSectionNotFoundException : Exception
    {
        public MenuSectionNotFoundException() {}
        public MenuSectionNotFoundException(string message): base(message) { }

        public MenuSectionNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
