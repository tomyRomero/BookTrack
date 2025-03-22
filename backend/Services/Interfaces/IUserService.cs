using backend.Models;
using backend.Models.DTOs;

namespace backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserRegistrationResponseDTO> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO);
        Task<User> GetUserByIdAsync(int id);
    }
}