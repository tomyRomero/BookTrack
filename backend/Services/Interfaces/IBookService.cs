using backend.Models;
using backend.Models.DTOs;

namespace backend.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookDTOResponse> CreateBookAsync(BookDTO bookDto);
        Task<BookDTOResponse?> GetBookByIdAsync(int bookId);
        Task<List<Book>> GetAllBooksAsync();
        Task<BookDTOResponse?> UpdateBookAsync(int bookId, BookDTO bookDto);
        Task<bool> DeleteBookAsync(int bookId);

        Task<List<BookDTOResponse>> GetBooksByUserAsync(int userId);

        Task<List<BookDTOResponse>> GetBooksNotByUserAsync(int userId);

    }
}