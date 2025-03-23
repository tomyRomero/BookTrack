using backend.Models;


namespace backend.Repositories.Interfaces;

    public interface IReviewRepository
    {
        Task<Review> CreateReviewAsync(Review review);
        Task<Review?> GetReviewAsync(int reviewId);
        Task<List<Review>> GetReviewsByUserAsync(int userId);
        Task<Review> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<Review?> GetReviewByBookIdAsync(int bookId);

        
    }
