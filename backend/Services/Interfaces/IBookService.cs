using backend.Models;
using backend.Models.DTOs;

namespace backend.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateBookAsync(BookDTO bookDto);
        Task<Book?> GetBookByIdAsync(int bookId);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> UpdateBookAsync(int bookId, BookDTO bookDto);
        Task<bool> DeleteBookAsync(int bookId);
    }
}