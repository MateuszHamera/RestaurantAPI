using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization
{
    public class ResourceOperationRequirementHandler
        : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            ResourceOperationRequirement requirement, 
            Restaurant restaurant)
        {
            if(requirement.Operation == Operation.Create
                || requirement.Operation == Operation.Read)
            {
                context.Succeed(requirement);
            }

            string? nameIdentifer = context.User
                ?.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(nameIdentifer))
            {
                var userId = int.Parse(nameIdentifer);

                if(restaurant.Id == userId)
                {
                    context.Succeed(requirement);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
