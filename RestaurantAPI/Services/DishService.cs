using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, DishDto dishDto);
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
        public int Create(int restaurantId, DishDto dishDto)
        {
            var restaurant = _context
                .Restaurants
                .SingleOrDefault(x => x.Id == restaurantId);

            if(restaurant == null)
                throw new NotFoundException("Dish not found");

            var dish = _mapper.Map<Dish>(dishDto);
            dish.RestaurantId = restaurantId;

            restaurant.Dishes.Add(dish);
            _context.SaveChanges();

            return dish.Id;
        }
    }
}
