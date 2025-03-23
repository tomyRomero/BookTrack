using backend.Models;
using backend.Models.DTOs;

namespace backend.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Review?> CreateReviewAsync(ReviewDTO reviewDto);
        Task<List<Review>> GetReviewsByUserAsync(int userId);
        Task<Review?> UpdateReviewAsync(int reviewId, string newContent);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}