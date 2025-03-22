

namespace backend.Models.DTOs;

public class UserResponseDTO
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; } // JWT token
}