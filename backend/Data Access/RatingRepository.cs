using backend.Data_Access;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> CreateRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating?> GetRatingAsync(int ratingId)
        {
            return await _context.Ratings.FindAsync(ratingId);
        }

  

        public async Task<List<Rating>> GetRatingsByUserAsync(int userId)
        {
            return await _context.Ratings
                                 .Where(r => r.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Rating> UpdateRatingAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<bool> DeleteRatingAsync(int ratingId)
        {
            var rating = await GetRatingAsync(ratingId);
            if (rating == null) return false;

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Rating?> GetRatingByBookIdAsync(int bookId)
        {
            return await _context.Ratings
                                .Where(r => r.BookId == bookId)
                                .FirstOrDefaultAsync();
        }
    }
}