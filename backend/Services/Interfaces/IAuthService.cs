using backend.Models.DTOs;

namespace backend.Services.Interfaces;

    public interface IAuthService
    {
        Task<UserResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync();
      
    }