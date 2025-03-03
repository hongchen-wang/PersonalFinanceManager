using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                return BadRequest("User already exists");
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            await _userRepository.AddUser(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var dbUser = _userRepository.GetUserByUsername(request.Username);
            
            if (dbUser == null || !VerifyPassword(dbUser.PasswordHash, request.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _jwtService.GenerateSecurityToken(request.Username);
            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {

            return _passwordHasher.HashPassword(null, password);
        }

        private bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, inputPassword) == PasswordVerificationResult.Success;
        }

    }
}
