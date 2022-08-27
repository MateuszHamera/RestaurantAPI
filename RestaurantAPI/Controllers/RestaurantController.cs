using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

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
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            return Ok(_restaurantService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute]int id)
        {
            var restaurant = _restaurantService.GetById(id);

            if(restaurant == null)
                return NotFound();

            var restaurantsDto = _mapper.Map<RestaurantDto>(restaurant);

            return Ok(restaurantsDto);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody]RestaurantDto restaurantDto)
        {
            var id = _restaurantService.Create(restaurantDto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

             return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody]UpdateRestaurantDto putRestaurantDto)
        {
            _restaurantService.Update(id, putRestaurantDto);

            return Ok();
        }
    }
}
