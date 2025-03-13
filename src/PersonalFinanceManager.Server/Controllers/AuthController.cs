using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManager.Server.Assets;
using PersonalFinanceManager.Server.Helpers;
using PersonalFinanceManager.Server.Models;
using PersonalFinanceManager.Server.Repositories;
using PersonalFinanceManager.Server.Services;
using LoginRequest = PersonalFinanceManager.Server.Models.LoginRequest;

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
            var refreshToken = TokenHelper.GenerateToken();
            dbUser.RefreshToken = refreshToken;
            dbUser.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUser(dbUser);

            // Set the refresh token in HttpOnly cookie
            Response.Cookies.Append(
                "refreshToken",
                refreshToken, 
                new CookieOptions 
                {
                    HttpOnly = true, // prevent JavaScript from accessing the cookie
                    Secure = true, // cookie will only be sent over HTTPS
                    SameSite = SameSiteMode.Strict, // cookie will not be sent on cross-site request forgery (prevent CSRF attacks)
                    Expires = DateTime.UtcNow.AddDays(7) // cookie will expire in 7 days
                });

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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // recover the refresh token from the cookie
            if (!Request.Cookies.TryGetValue("refreshToke", out var refreshToken))
            { 
                return Unauthorized(Ressources.BusinessException.NoRefreshTokenFound);
            }

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
            var accessToken = _jwtService.GenerateSecurityToken(dbUser);

            // generate refresh token
            var newRefreshToken = TokenHelper.GenerateToken();
            dbUser.RefreshToken = newRefreshToken;
            dbUser.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUser(dbUser);

            // Set the refresh token in HttpOnly cookie
            Response.Cookies.Append(
                "refreshToken",
                newRefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

            return Ok(new { Token = accessToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Ok(Ressources.BusinessException.NoRefreshTokenFound);
            }

            var dbUser = _userRepository.GetUserByRefreshToken(refreshToken);
            if (dbUser != null)
            {
                // remove the refresh token from db
                dbUser.RefreshToken = null;
                dbUser.RefreshTokenExpiration = null;
                await _userRepository.UpdateUser(dbUser);
            }

            // remove the refresh token from the cookie
            Response.Cookies.Delete("refreshToken");
            return Ok(new { Message = "Logged out successfully" });
        }

        // reset password 
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordResetRequest request)
        { 
            var dbUser = _userRepository.GetUserByEmail(request.Email);
            if (dbUser == null)
            { 
                return BadRequest(Ressources.BusinessException.UserNotFound);
            }

            // generate reset token
            dbUser.ResetToken = TokenHelper.GenerateToken();
            dbUser.ResetTokenExpiration = DateTimeOffset.UtcNow.AddHours(1); // reset token valid for 1h
            await _userRepository.UpdateUser(dbUser);

            // TODO: Send email with reset link (e.g., "https://frontend.com/reset?token={user.ResetToken}")

            return Ok(new { Message = Ressources.ResultMessage.ResetLinkSent });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
        { 
            var dbUser = _userRepository.GetUserByResetToken(request.Token);
            if (dbUser == null || dbUser.ResetTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest(Ressources.BusinessException.ResetTokenExpired);
            }

            // hash new password for storage
            dbUser.PasswordHash = HashPassword(request.NewPassword);

            // clear reset token
            dbUser.ResetToken = null;
            dbUser.ResetTokenExpiration = null;
            await _userRepository.UpdateUser(dbUser);

            return Ok(new { Message = Ressources.ResultMessage.PasswordReset });
        }

    }
}
