using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        public int[] allowedPageSize = new[] { 5, 10, 15 };
        public string[] allowedSortByColumnName = new[]
        {   nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description)};
        public RestaurantQueryValidator()
        {
            RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(q => q.PageSize).Custom((value, context) =>
            {
                if(!allowedPageSize.Contains(value))
                {
                    context.AddFailure("PageSize", $"Page Size must be in {string.Join(",",allowedPageSize)}");
                }    
            });
        }
    }
}
