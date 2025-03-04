using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManager.Server.Assets;
using PersonalFinanceManager.Server.Helpers;
using PersonalFinanceManager.Server.Models;
using PersonalFinanceManager.Server.Repositories;
using PersonalFinanceManager.Server.Services;

namespace PersonalFinanceManager.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthController(UserRepository userRepository,
            JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (_userRepository.GetUserByUsername(user.Username) != null)
            {
                return BadRequest(Ressources.BusinessException.UserAlreadyExists);
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            await _userRepository.AddUser(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var dbUser = _userRepository.GetUserByUsername(request.Username);
            
            if (dbUser == null || !VerifyPassword(dbUser.PasswordHash, request.Password))
            {
                return Unauthorized(Ressources.BusinessException.InvalidCredentials);
            }

            var token = _jwtService.GenerateSecurityToken(dbUser);

            // generate refresh token
            var refreshToken = TokenHelper.GenerateRefreshToken();
            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUser(dbUser);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        private string HashPassword(string password)
        {

            return _passwordHasher.HashPassword(null, password);
        }

        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, inputPassword) == PasswordVerificationResult.Success;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var dbUser = _userRepository.GetUserByRefreshToken(request.RefreshToken);
            if (dbUser == null)
            {
                return Unauthorized(Ressources.BusinessException.InvalidRefreshToken);
            }

            if (dbUser != null && dbUser.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return Unauthorized(Ressources.BusinessException.RefreshTokenExpired);
            }

            // generate a new access token
            var token = _jwtService.GenerateSecurityToken(dbUser);

            // generate refresh token
            var refreshToken = TokenHelper.GenerateRefreshToken();
            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUser(dbUser);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

    }
}
