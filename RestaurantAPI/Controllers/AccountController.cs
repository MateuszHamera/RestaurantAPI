using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterUserDto registerUserDto)
        {
            await _accountService.RegisterUserAsync(registerUserDto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginUserDto loginUserDto)
        {
            string token = await _accountService.LoginAsync(loginUserDto);

            return Ok(token);
        }

        [HttpPost("{userId}/role/{roleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRoleForUserAsync([FromRoute]int userId, [FromRoute]int roleId)
        {
            await _accountService.ChangeRoleForUserAsync(userId, roleId);

            return Ok();
        }
    }
}
