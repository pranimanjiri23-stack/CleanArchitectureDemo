using CleanArchitectureDemo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitectureDemo.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // Demo user
            if (request.Username != "admin" ||
                request.Password != "admin123")
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            var token = _jwtTokenService.GenerateToken(
                request.Username,
                "Admin");

            return Ok(new
            {
                accessToken = token
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
