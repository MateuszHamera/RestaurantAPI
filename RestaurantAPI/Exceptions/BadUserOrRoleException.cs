namespace RestaurantAPI.Exceptions
{
    public class BadUserOrRoleException : Exception
    {
        public BadUserOrRoleException(string message) : base(message)
        {

        }
    }
}
