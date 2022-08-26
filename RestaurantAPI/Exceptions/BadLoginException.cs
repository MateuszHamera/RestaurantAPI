namespace RestaurantAPI.Exceptions
{
    public class BadLoginException : Exception
    {
        public BadLoginException(string message) : base(message)
        {

        }
    }
}
