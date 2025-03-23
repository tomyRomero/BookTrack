using backend.Data_Access;
using backend.Models;
using backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace backend.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> GetReviewAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

        public async Task<List<Review>> GetReviewsByUserAsync(int userId)
        {
            return await _context.Reviews
                                 .Where(r => r.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Review> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await GetReviewAsync(reviewId);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Review?> GetReviewByBookIdAsync(int bookId)
        {
            return await _context.Reviews
                                .Where(r => r.BookId == bookId)
                                .FirstOrDefaultAsync();
        }

    }
}