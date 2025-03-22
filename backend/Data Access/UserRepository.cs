using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Models.DTOs;
using backend.Repositories.Interfaces;
using System.Threading.Tasks;
using backend.Data_Access;
namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddBookAsync(int userId, Book book)
   {

        var user = await _context.Users
                .Include(u => u.Books) 
                .FirstOrDefaultAsync(u => u.UserId == userId) ?? throw new KeyNotFoundException("User not found.");
            
            // Initialize Books if null
            user.Books ??= new HashSet<Book>();

            user.Books.Add(book); 

            await _context.SaveChangesAsync();
    }

}

