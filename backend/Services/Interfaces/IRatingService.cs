using backend.Models;
using backend.Models.DTOs;

namespace backend.Services.Interfaces;

    public interface IRatingService
    {
        Task<Rating?> CreateRatingAsync(RatingDTO ratingDto);
        Task<List<Rating>> GetRatingsByBookAsync(int bookId);
        Task<List<Rating>> GetRatingsByUserAsync(int userId);
        Task<Rating?> UpdateRatingAsync(int ratingId, int newScore);
        Task<bool> DeleteRatingAsync(int ratingId);
    }
