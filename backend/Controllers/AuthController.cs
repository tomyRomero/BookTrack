using Microsoft.AspNetCore.Mvc;
using backend.Services.Interfaces;
using backend.Models.DTOs;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Authenticate and get JWT
                var userResponseDTO = await _authService.LoginAsync(loginDTO);                
                // Return the JWT inside a user response DTO
                return Ok(userResponseDTO);
            }
            catch (Exception ex)
            {
                // Handle any authentication errors
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
             try
            {
                // Invalidate the current user's JWT
                await _authService.LogoutAsync();
                // Return success
                return Ok(new { Message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                // Handle any errors during logout
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
