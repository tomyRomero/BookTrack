using backend.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace backend.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> CreateBookAsync(Book book);
        Task<Book?> GetBookByIdAsync(int bookId);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int bookId);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<List<Book>> GetBooksNotByUserIdAsync(int userId);
        Task<List<Book>> GetBooksByUserIdAsync(int userId);

        
    }
}