
namespace backend.Models.DTOs;

public class UserRegistrationDTO
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class UserRegistrationResponseDTO
{
    public int UserId { get; set; }
    public string? Username { get; set; }

}

public class UserResponseDTO
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; } // JWT token
}