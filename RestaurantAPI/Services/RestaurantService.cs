using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Authorization;
using System.Linq.Expressions;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantDto> GetByIdAsync(int id);
        Task<PageResult<RestaurantDto>> GetAllAsync(RestaurantQuery query);
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

        public async Task<PageResult<RestaurantDto>> GetAllAsync(RestaurantQuery query)
        {
            var baseQuery = _restaurantDbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .Where(r => query.SearchPhrase == null
                || r.Name.ToLower().Contains(query.SearchPhrase)
                || r.Description.Contains(query.SearchPhrase));

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.Category), r => r.Category }
                };

                var selectorColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.Ascending ?
                    baseQuery.OrderBy(selectorColumn) :
                    baseQuery.OrderByDescending(q => q.Name);
            }

            var restaurants = await baseQuery
                .Skip(query.PageNumber * (query.PageSize - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PageResult<RestaurantDto>(
                restaurantsDto, 
                query.PageNumber, 
                query.PageSize, 
                baseQuery.Count());

            return result;
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

