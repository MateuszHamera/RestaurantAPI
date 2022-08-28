using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        IEnumerable<RestaurantDto> GetAll();
        int Create(RestaurantDto restaurantDto, int userId);
        void Delete(int id, ClaimsPrincipal user);
        void Update(int id, UpdateRestaurantDto updateRestaurantDto, ClaimsPrincipal user);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantService(
            RestaurantDbContext restaurantDbContext,
            IMapper mapper,
            ILogger<RestaurantService> logger,
            IAuthorizationService authorizationService
        )
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
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

        public int Create(RestaurantDto restaurantDto, int userId)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantDto);
            restaurant.CreatedById = userId;

            _restaurantDbContext.Add(restaurant);
            _restaurantDbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .SingleOrDefault(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var authorization = _authorizationService.AuthorizeAsync(user, restaurant,
                new ResourceOperationRequirement(Operation.Update)).Result;

            if(!authorization.Succeeded)
                throw new ForbidException("Can't update restaurant");

            _restaurantDbContext.Remove(restaurant);
            _restaurantDbContext.SaveChanges();
        }

        public void Update(int id, 
            UpdateRestaurantDto updateRestaurantDto,
            ClaimsPrincipal user)
        {
            var restaurant = _restaurantDbContext
                .Restaurants
                .SingleOrDefault(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var authorization = _authorizationService.AuthorizeAsync(user, restaurant,
                new ResourceOperationRequirement(Operation.Update)).Result;

            if(!authorization.Succeeded)
                throw new ForbidException("Can't update restaurant");

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            _restaurantDbContext.SaveChanges();
        }
    }
}

