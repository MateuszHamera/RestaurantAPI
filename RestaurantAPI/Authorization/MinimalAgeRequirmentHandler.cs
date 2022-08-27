using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimalAgeRequirmentHandler : AuthorizationHandler<MinimalAgeRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalAgeRequirment requirement)
        {
            var dateofBirth = DateTime.Parse(context.User.FindFirst(u => u.Type == "DateOfBirth").Value);

            if(dateofBirth.AddYears(requirement.MinAge) < DateTime.Today)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
