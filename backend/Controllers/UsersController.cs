using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Services.Interfaces;
using backend.Models.DTOs;
using System.Threading.Tasks;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDTO)
    {
        // Registration logic
        if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userRegistrationResponseDTO = await _userService.RegisterUserAsync(userRegistrationDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = userRegistrationResponseDTO.UserId }, userRegistrationResponseDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
    }


    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        //Get user by ID logic
           var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
    }

}
