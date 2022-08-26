namespace RestaurantAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int Email { get; set; }
        public string FirstName { get; set; }
        public int LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
