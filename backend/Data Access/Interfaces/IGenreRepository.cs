using backend.Models;

namespace backend.Repositories
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int genreId);
        Task<Genre> UpdateGenreAsync(Genre genre);
        Task<Genre> AddGenreAsync(Genre genre);
        Task<(bool Success, string Message)> DeleteGenreAsync(int genreId);
    }
}