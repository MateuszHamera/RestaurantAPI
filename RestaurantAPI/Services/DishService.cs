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
        int Create(int restaurantId, DishDto dishDto);
        DishDto GetById(int restaurantId, int dishId);
        IEnumerable<DishDto> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
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
        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _context.Dishes
                ?.SingleOrDefault(d => d.Id == dishId);

            if(dish == null)
                throw new NotFoundException("Dish not found");

            return _mapper.Map<DishDto>(dish);
        }

        public IEnumerable<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishes = _context.Dishes;

            if(!dishes.Any())
                throw new NotFoundException("Dishes not found");

            return _mapper.Map<IEnumerable<DishDto>>(dishes);
        }

        public int Create(int restaurantId, DishDto dishDto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dishDto);
            dish.RestaurantId = restaurantId;

            restaurant.Dishes.Add(dish);
            _context.SaveChanges();

            return dish.Id;
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _context.Remove(restaurant.Dishes);
            _context.SaveChanges();
        }

        public Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _context
                ?.Restaurants
                ?.SingleOrDefault(x => x.Id == restaurantId);

            if(restaurant == null)
                throw new NotFoundException("Dish not found");

            return restaurant;
        }
    }
}
