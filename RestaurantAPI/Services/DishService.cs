using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        Task<int> CreateAsync(int restaurantId, DishDto dishDto);
        Task<DishDto> GetByIdAsync(int restaurantId, int dishId);
        Task<IEnumerable<DishDto>> GetAllAsync(int restaurantId);
        Task RemoveAllAsync(int restaurantId);
    }
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<DishDto> GetByIdAsync(int restaurantId, int dishId)
        {
            var restaurant = await GetRestaurantById(restaurantId);

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(d => d.Id == dishId);

            if(dish == null)
                throw new NotFoundException("Dish not found");

            return _mapper.Map<DishDto>(dish);
        }

        public async Task<IEnumerable<DishDto>> GetAllAsync(int restaurantId)
        {
            var restaurant = await GetRestaurantById(restaurantId);

            var dishes = _context.Dishes;

            if(!dishes.Any())
                throw new NotFoundException("Dishes not found");

            return _mapper.Map<IEnumerable<DishDto>>(dishes);
        }

        public async Task<int> CreateAsync(int restaurantId, DishDto dishDto)
        {
            var restaurant = await GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dishDto);
            dish.RestaurantId = restaurantId;

            restaurant.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return dish.Id;
        }

        public async Task RemoveAllAsync(int restaurantId)
        {
            var restaurant = await GetRestaurantById(restaurantId);

            _context.Remove(restaurant.Dishes);
            await _context.SaveChangesAsync();
        }

        public async Task<Restaurant> GetRestaurantById(int restaurantId)
        {
            var restaurant = await _context
                .Restaurants
                .SingleOrDefaultAsync(x => x.Id == restaurantId);

            if(restaurant == null)
                throw new NotFoundException("Dish not found");

            return restaurant;
        }
    }
}
