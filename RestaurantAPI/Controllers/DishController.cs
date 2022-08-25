using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("/api/restaurant/{restaurantId}/dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> GetById([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            return Ok(_dishService.GetById(restaurantId, dishId));
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            return Ok(_dishService.GetAll(restaurantId));
        }

        [HttpPost]
        public ActionResult Create([FromRoute]int restaurantId, [FromBody]DishDto dishDto)
        {
            var id = _dishService.Create(restaurantId, dishDto);

            return Created($"/api/{restaurantId}/dish/{id}", null);
        }

        [HttpDelete]
        public ActionResult DeleteAll([FromRoute] int restaurantId)
        {
            _dishService.RemoveAll(restaurantId);

            return NoContent();
        }
    }
}
