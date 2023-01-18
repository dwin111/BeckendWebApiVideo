using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestWebApiOnlineMove.Common;
using TestWebApiOnlineMove.Context;
using TestWebApiOnlineMove.Models;

namespace TestWebApiOnlineMove.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context,IOptions<AuthOptions> options)
        {
            _authOptions = options;
            _context = context;
        }
        [HttpPost("registr")]
        public IActionResult Registr(Account request)
        {
            var user = new Account
            {
                id = 0,
                Email = request.Email,
                Password = request.Password,
                Roles = new Role[] {Role.Admin}
            };
            if (user != null)
            {
                _context.Accounts.Add(user);
                _context.SaveChanges();

                return Ok(user);
            }
            return Unauthorized();
        }

        [HttpPost("login")]
        public IActionResult Login(Login request)
        {
            Account user = AuthenticateUser(request.Email, request.Password);
            if(user != null)
            {
                var token = GenerJWTToken(user);

                return Ok(new { access_token = token, id = user.id});
            }
            return Unauthorized();
        }

        private Account AuthenticateUser(string email, string password)
        {
            var user = _context.Accounts.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }
        private string GenerJWTToken(Account user)
        {
            var authParams = _authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.id.ToString())
            };

            foreach(var role in user.Roles)
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
