using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimalAgeRequirmentHandler : AuthorizationHandler<MinimalAgeRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimalAgeRequirment requirement)
        {
            var date = context.User.FindFirst(u => u.Type == "DateOfBirth")?.Value;

            if(!string.IsNullOrEmpty(date))
            {
                var dateofBirth = DateTime.Parse(date);

                if(dateofBirth.AddYears(requirement.MinAge) < DateTime.Today)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
