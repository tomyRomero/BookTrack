using backend.Models;
using backend.Models.DTOs;

namespace backend.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDTO>> GetAllGenresAsync();
        Task<GenreDTO?> GetGenreByIdAsync(int genreId);
        Task<GenreResponseDTO?> AddGenreAsync(GenreDTO genreDTO);
        Task<GenreResponseDTO?> UpdateGenreAsync(int genreId, GenreDTO genreDTO);
        Task<(bool Success, string Message)> DeleteGenreAsync(int genreId);
    }
}