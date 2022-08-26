using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountService(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
        }
        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var test = "test";
        }
    }
}
