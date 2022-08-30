using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimalAgeRequirment : IAuthorizationRequirement
    {
        public int MinAge { get; init; }

        public MinimalAgeRequirment(int minAge)
        {
            MinAge = minAge;
        }
    }
}
