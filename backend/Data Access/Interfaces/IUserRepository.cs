using backend.Models;
using backend.Models.DTOs;
using System.Threading.Tasks;

namespace backend.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int userId);
    Task AddBookAsync(int userId, Book book);
}

