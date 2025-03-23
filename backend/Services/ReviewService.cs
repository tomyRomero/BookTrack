using backend.Models.DTOs;
using backend.Models;
using backend.Repositories;
using backend.Services.Interfaces;
using backend.Repositories.Interfaces;

namespace backend.Services
{
    public class ReviewService : IReviewService
    {
         private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review?> CreateReviewAsync(ReviewDTO reviewDto)
        {
            // Check if a review already exists for this user and book
            var existingReview = await _reviewRepository.GetReviewsByUserAsync(reviewDto.UserId);
            if (existingReview.Any(r => r.BookId == reviewDto.BookId))
            {
                throw new Exception("User has already reviewed this book.");
            }

            // Create a new review
            var review = new Review
            {
                BookId = reviewDto.BookId,
                UserId = reviewDto.UserId,
                Content = reviewDto.Content
            };

            return await _reviewRepository.CreateReviewAsync(review);
        }


        public async Task<List<Review>> GetReviewsByUserAsync(int userId)
        {
            return await _reviewRepository.GetReviewsByUserAsync(userId);
        }

        public async Task<Review?> UpdateReviewAsync(int reviewId, string newContent)
        {
            var review = await _reviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
                throw new Exception("Review not found.");
            }

            review.Content = newContent;

            return await _reviewRepository.UpdateReviewAsync(review);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            return await _reviewRepository.DeleteReviewAsync(reviewId);
        }
    }
}