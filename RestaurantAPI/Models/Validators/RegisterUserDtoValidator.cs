using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext restaurantDtoContext)
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress();
            RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
            RuleFor(r => r.ComfirmPassword).Equal(c => c.Password);

            RuleFor(r => r.Email)
                .Custom((value, context) =>
                {
                    if(restaurantDtoContext.Users.Any(u => u.Email == value))
                        context.AddFailure("Email", "That email is taken");
                });
        }
    }
}
