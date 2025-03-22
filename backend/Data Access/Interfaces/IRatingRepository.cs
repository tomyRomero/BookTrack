using backend.Models;


namespace backend.Repositories.Interfaces;

    public interface IRatingRepository
    {
        Task<Rating> CreateRatingAsync(Rating rating);
        Task<Rating?> GetRatingAsync(int ratingId);
        Task<List<Rating>> GetRatingsByBookAsync(int bookId);
        Task<List<Rating>> GetRatingsByUserAsync(int userId);
        Task<Rating> UpdateRatingAsync(Rating rating);
        Task<bool> DeleteRatingAsync(int ratingId);
    }
