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

        [HttpPost]
        public ActionResult Create([FromRoute]int restaurantId, [FromBody]DishDto dishDto)
        {
            var id = _dishService.Create(restaurantId, dishDto);

            return Created($"/api/{restaurantId}/dish/{id}", null);
        }
    }
}
