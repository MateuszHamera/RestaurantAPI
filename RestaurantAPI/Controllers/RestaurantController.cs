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
        [Authorize(Policy = "Min20Years")]
        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            return await _restaurantService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetAsync([FromRoute]int id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);

            if(restaurant == null)
                return NotFound();

            var restaurantsDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(restaurantsDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRestaurantAsync([FromBody]RestaurantDto restaurantDto)
        {
            var userId = User?.FindFirst(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

            var id = await _restaurantService.CreateAsync(restaurantDto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _restaurantService.DeleteAsync(id);

             return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody]UpdateRestaurantDto putRestaurantDto)
        {
            await _restaurantService.UpdateAsync(id, putRestaurantDto);

            return Ok();
        }
    }
}
