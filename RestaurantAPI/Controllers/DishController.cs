using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [Authorize]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet("{dishId}")]
        [AllowAnonymous]
        public async Task<ActionResult<DishDto>> GetByIdAsync([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            return Ok(await _dishService.GetByIdAsync(restaurantId, dishId));
        }

        [HttpGet]
        [Authorize(Policy = "HasNationality")]
        public async Task<IEnumerable<DishDto>> GetAllAsync([FromRoute] int restaurantId)
        {
            return await _dishService.GetAllAsync(restaurantId);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute]int restaurantId, [FromBody]DishDto dishDto)
        {
            var id = await _dishService.CreateAsync(restaurantId, dishDto);

            return Created($"/api/{restaurantId}/dish/{id}", null);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAll([FromRoute] int restaurantId)
        {
            await _dishService.RemoveAllAsync(restaurantId);

            return NoContent();
        }
    }
}
