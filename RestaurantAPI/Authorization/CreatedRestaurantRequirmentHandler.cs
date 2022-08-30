using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class CreatedRestaurantRequirmentHandler : AuthorizationHandler<CreatedRestaurantRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedRestaurantRequirment requirement)
        {
            var createdRestaurant = context.User.FindFirst(u => u.Type == "CreatedRestaurants")?.Value;

            if(!string.IsNullOrEmpty(createdRestaurant))
            {
                var restaurantsCount = int.Parse(createdRestaurant);

                if(restaurantsCount >= requirement.CreatedRestaurant)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;

        }
    }
}
