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
        Task<RestaurantDto> GetByIdAsync(int id);
        Task<IEnumerable<RestaurantDto>> GetAllAsync(RestaurantQuery query);
        Task<int> CreateAsync(RestaurantDto restaurantDto);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateRestaurantDto updateRestaurantDto);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(
            RestaurantDbContext restaurantDbContext,
            IMapper mapper,
            ILogger<RestaurantService> logger,
            IAuthorizationService authorizationService,
            IUserContextService userContextService
        )
        {
            _restaurantDbContext = restaurantDbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task<RestaurantDto> GetByIdAsync(int id)
        {
            var restaurant = await _restaurantDbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .SingleOrDefaultAsync(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync(RestaurantQuery query)
        {
            var restaurants = await _restaurantDbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .Where(r => query.SearchPhrase == null 
                || r.Name.ToLower().Contains(query.SearchPhrase) 
                || r.Description.Contains(query.SearchPhrase))
                .Skip(query.PageNumber * (query.PageSize - 1))
                .Take(query.PageSize)
                .ToListAsync();

            return _mapper.Map<List<RestaurantDto>>(restaurants);
        }

        public async Task<int> CreateAsync(RestaurantDto restaurantDto)
        {
            var restaurant = _mapper.Map<Restaurant>(restaurantDto);
            restaurant.CreatedById = _userContextService.UserId;

            await _restaurantDbContext.AddAsync(restaurant);
            await _restaurantDbContext.SaveChangesAsync();

            return restaurant.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var restaurant = await _restaurantDbContext
                .Restaurants
                .SingleOrDefaultAsync(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var authorization = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationRequirement(Operation.Delete)).Result;

            if(!authorization.Succeeded)
                throw new ForbidException("Can't update restaurant");

            _restaurantDbContext.Remove(restaurant);
            await _restaurantDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateRestaurantDto updateRestaurantDto)
        {
            var restaurant = await _restaurantDbContext
                .Restaurants
                .SingleOrDefaultAsync(x => x.Id == id);

            if(restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var authorization = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationRequirement(Operation.Update)).Result;

            if(!authorization.Succeeded)
                throw new ForbidException("Can't update restaurant");

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            await _restaurantDbContext.SaveChangesAsync();
        }
    }
}

