using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(
            IRestaurantService restaurantService,
            IMapper mapper,
            ILogger<RestaurantController> logger
        )
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<RestaurantDto>> GetAllAsync([FromQuery]string searchPhrase)
        {
            return await _restaurantService.GetAllAsync(searchPhrase);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CreatedRestaurants")]
        public async Task<ActionResult<RestaurantDto>> GetAsync(int id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);

            if(restaurant == null)
                return NotFound();

            var restaurantsDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(restaurantsDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRestaurantAsync(RestaurantDto restaurantDto)
        {
            var userId = User?.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

            var id = await _restaurantService.CreateAsync(restaurantDto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _restaurantService.DeleteAsync(id);

             return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, UpdateRestaurantDto putRestaurantDto)
        {
            await _restaurantService.UpdateAsync(id, putRestaurantDto);

            return Ok();
        }
    }
}
