using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class CreatedRestaurantRequirmentHandler : AuthorizationHandler<CreatedRestaurantRequirment>
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        public CreatedRestaurantRequirmentHandler(RestaurantDbContext restaurantDbContext)
        {
            _restaurantDbContext = restaurantDbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedRestaurantRequirment requirement)
        {
            var id = int.Parse(context.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)?.Value);

            var createdRestaurants = _restaurantDbContext
                .Restaurants.Count(r => r.Id == id);

            if(createdRestaurants > requirement.MinimumCreatedRestaurant)
            {
                context.Succeed(requirement);
            }    

            return Task.CompletedTask;
        }
    }
}
