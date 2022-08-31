using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedRestaurantRequirment : IAuthorizationRequirement
    {
        public int MinimumCreatedRestaurant { get; init; }
        public CreatedRestaurantRequirment(int minimumCreatedRestaurant)
        {
            MinimumCreatedRestaurant = minimumCreatedRestaurant;
        }
    }
}
