﻿using Microsoft.AspNetCore.Authorization;
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
        public ActionResult Register([FromBody]RegisterUserDto registerUserDto)
        {
            _accountService.RegisterUser(registerUserDto);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginUserDto loginUserDto)
        {
            string token = _accountService.Login(loginUserDto);

            return Ok(token);
        }

        [HttpPost("{userId}/role/{roleId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRoleForUser([FromRoute]int userId, [FromRoute]int roleId)
        {
            _accountService.ChangeRoleForUser(userId, roleId);

            return Ok();
        }
    }
}