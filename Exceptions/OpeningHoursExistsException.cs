namespace Sufra_MVC.Exceptions
{
    public class OpeningHoursExistsException : Exception
    {
        public OpeningHoursExistsException() {}
        public OpeningHoursExistsException(string message): base(message) { }

        public OpeningHoursExistsException(string message, Exception inner) : base(message, inner) { }
    }
}
