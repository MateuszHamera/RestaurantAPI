using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _restaurantDbContext;

        public RestaurantSeeder(RestaurantDbContext restaurantDbContext)
        {
            _restaurantDbContext = restaurantDbContext;
        }

        public void Seed()
        {
            if(_restaurantDbContext.Database.CanConnect())
            {
                if(!_restaurantDbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();

                    _restaurantDbContext.Restaurants.AddRange(restaurants);
                    _restaurantDbContext.SaveChanges();
                }

                if(!_restaurantDbContext.Roles.Any())
                {
                    IEnumerable<Role> roles = GetRoles();

                    _restaurantDbContext.Roles.AddRange(roles);
                    _restaurantDbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role() { Name = "User" },
                new Role() { Name = "Manager" },
                new Role() { Name = "Admin" },
            };
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            return new List<Restaurant>()
            { 
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "FastFod",
                    Description = "Fantastish",
                    ContactEmail = "kfc@abc",
                    ContactNumber = "213",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "chicken",
                            Description = "delicious",
                            Price = 100
                        },
                        new Dish()
                        {
                            Name = "ice",
                            Description = "delicious",
                            Price = 20
                        }
                    },
                    Address = new Address()
                    {
                        Street = "abc",
                        City = "BB",
                        PostalCode = "43-300"
                    }
                },
                new Restaurant()
                {
                    Name = "Mac",
                    Category = "FastFod",
                    Description = "Fantastish nr 2",
                    ContactEmail = "mac@abc",
                    ContactNumber = "321",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "chicken",
                            Description = "delicious",
                            Price = 2100
                        },
                        new Dish()
                        {
                            Name = "ice",
                            Description = "delicious",
                            Price = 220
                        }
                    },
                    Address = new Address()
                    {
                        Street = "acb",
                        City = "B-B",
                        PostalCode = "43-305"
                    }
                }
            };
        }
    }
}
