using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<string> LoginAsync(LoginUserDto loginUserDto);
        Task ChangeRoleForUserAsync(int userId, int roleId);
    }
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(
            RestaurantDbContext context,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper,
            AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _authenticationSettings = authenticationSettings;
        }

        public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var user = _mapper.Map<User>(registerUserDto);

            user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);
            user.RoleId = 1;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeRoleForUserAsync(int userId, int roleId)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .SingleOrDefault(u => u.Id == userId);

            if(user is null)
                throw new BadUserOrRoleException("Invalid userId or roleId");

            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == roleId);

            if(role is null)
                throw new BadUserOrRoleException("Invalid userId or roleId");

            user.RoleId = roleId;
            user.Role = role;

            await _context.SaveChangesAsync();                    
        }
        public async Task<string> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _context.Users
                .Include(c=> c.Role)
                .SingleOrDefaultAsync(u => u.Email == loginUserDto.Email);

            if(user is null)
                throw new BadLoginException("Invalid email or password");

            var result = _passwordHasher
                .VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);

            if(result is PasswordVerificationResult.Failed)
                throw new BadLoginException("Invalid email or password");

            var createdRestaurants = await _context.Restaurants
                .Where(r => r.CreatedById == user.Id).CountAsync();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.ToString("yyyyy-MM-dd"))
            };

            if(!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(new Claim("Nationality", $"{user.Nationality}"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(issuer: _authenticationSettings.JwtIssuer,
                audience: _authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: credential);

            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }
    }
}
