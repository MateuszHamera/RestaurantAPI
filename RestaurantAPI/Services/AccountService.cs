using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
    }
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public AccountService(
            RestaurantDbContext restaurantDbContext,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper)
        {
            _restaurantDbContext = restaurantDbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
        public void RegisterUser(RegisterUserDto registerUserDto)
        {

        }
    }
}
