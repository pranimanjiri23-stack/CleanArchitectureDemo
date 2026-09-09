using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Infrastructure.Data;
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
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthController(
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (request.Username != "admin" ||
                request.Password != "admin123")
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            var role = "Admin";

            var accessToken = _jwtTokenService.GenerateToken(
                request.Username,
                role);

            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Username = request.Username,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
    RefreshTokenRequest request)
        {
            var oldRefreshToken = await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken);

            if (oldRefreshToken == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid refresh token."
                });
            }

            if (oldRefreshToken.RevokedAt != null)
            {
                return Unauthorized(new
                {
                    message = "Refresh token has been revoked."
                });
            }

            if (oldRefreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    message = "Refresh token has expired."
                });
            }

            // Revoke the old refresh token
            await _refreshTokenRepository
                .RevokeAsync(oldRefreshToken);

            // Generate new access token
            var accessToken = _jwtTokenService.GenerateToken(
                oldRefreshToken.Username,
                "Admin");

            // Generate new refresh token
            var newRefreshToken =
                _refreshTokenService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                Username = oldRefreshToken.Username,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            // Save new refresh token
            await _refreshTokenRepository
                .AddAsync(newRefreshTokenEntity);

            return Ok(new
            {
                accessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequest request)
        {
            var refreshToken = await _refreshTokenRepository
                .GetByTokenAsync(request.RefreshToken);

            if (refreshToken == null)
            {
                return NotFound(new
                {
                    message = "Refresh token not found."
                });
            }

            if (refreshToken.RevokedAt != null)
            {
                return Ok(new
                {
                    message = "Refresh token is already revoked."
                });
            }

            await _refreshTokenRepository.RevokeAsync(refreshToken);

            return Ok(new
            {
                message = "Logout successful."
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
