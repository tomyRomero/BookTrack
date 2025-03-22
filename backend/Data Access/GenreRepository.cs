using backend.Data_Access;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int genreId)
        {
           return await _context.Genres
            .Where(g => g.GenreId == genreId)
            .FirstOrDefaultAsync();
        }

        public async Task<Genre> AddGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> UpdateGenreAsync(Genre genre)
       {
        _context.Genres.Update(genre); // Mark the genre as updated
        await _context.SaveChangesAsync(); 
        return genre; 
        }

        public async Task<(bool Success, string Message)> DeleteGenreAsync(int genreId)
        {
            // Check if the genre exists and if it has associated books
            var genre = await _context.Genres
                .Where(g => g.GenreId == genreId)
                .FirstOrDefaultAsync();

            // Case 1: Genre not found
            if (genre == null)
            {
                return (false, "Genre not found."); 
            }

            // Case 2: Genre has associated books
            if (genre.Books != null && genre.Books.Count > 0)
            {
                return (false, "Cannot delete genre because it has books assigned.");
            }

            // Case 3: Genre exists and has no books, proceed with deletion
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return (true, "Genre deleted successfully.");
        }
    }
}