using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public enum Operation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(Operation operation)
        {
            Operation = operation;
        }

        public Operation Operation { get; }
    }
}
