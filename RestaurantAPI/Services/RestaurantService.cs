using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(RestaurantDto restaurantDto);
        void Delete(int id);
        void Update(int id, UpdateRestaurantDto putRestaurantDto);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(
            RestaurantDbContext restaurantDbContext,
            IMapper mapper,
            ILogger<RestaurantService> logger
        )
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .SingleOrDefault(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found"); 

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _restaurantDbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .ToList();

            return _mapper.Map<List<RestaurantDto>>(restaurants);
        }

        public int Create(RestaurantDto restaurantDto)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantDto);

            _restaurantDbContext.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .SingleOrDefault(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            _restaurantDbContext.Remove(restaurant);
            _restaurantDbContext.SaveChanges();
        }

        public void Update(int id, UpdateRestaurantDto putRestaurantDto)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .SingleOrDefault(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            restaurant.Name = putRestaurantDto.Name;
            restaurant.Description = putRestaurantDto.Description;
            restaurant.HasDelivery = putRestaurantDto.HasDelivery;

            _restaurantDbContext.SaveChanges();
        }
    }
}

