using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManager.Server.Models;
using PersonalFinanceManager.Server.Services;

namespace PersonalFinanceManager.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // TODO: replace Email by Username (call from user service ?)
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _jwtService.GenerateSecurityToken(request.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid credentials");
        }

        //private readonly IUserService _userService;
        //private readonly JwtService _jwtService;
        //public AuthController(IUserService userService, JwtService jwtService)
        //{
        //    _userService = userService;
        //    _jwtService = jwtService;
        //}
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest request)
        //{
        //    var user = await _userService.GetUserByUsername(request.Username);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        //    {
        //        return Unauthorized();
        //    }
        //    var token = _jwtService.GenerateSecurityToken(user.Username);
        //    return Ok(new { Token = token });
        //}
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        //{
        //    var user = await _userService.GetUserByUsername(request.Username);
        //    if (user != null)
        //    {
        //        return BadRequest("User already exists");
        //    }
        //    var newUser = new User
        //    {
        //        Username = request.Username,
        //        Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        //    };
        //    await _userService.AddUser(newUser);
        //    return Ok();
        //}
    }
}
