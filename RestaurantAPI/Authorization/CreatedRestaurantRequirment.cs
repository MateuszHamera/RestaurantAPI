using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedRestaurantRequirment : IAuthorizationRequirement
    {
        public int CreatedRestaurant { get; init; }
        public CreatedRestaurantRequirment(int createdRestaurant)
        {
            CreatedRestaurant = createdRestaurant;
        }
    }
}
