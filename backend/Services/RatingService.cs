
using backend.Models;
using backend.Models.DTOs;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;


namespace backend.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<Rating?> CreateRatingAsync(RatingDTO ratingDto)
        {
            // Check if a rating already exists for this user and book
            var existingRatings = await _ratingRepository.GetRatingsByUserAsync(ratingDto.UserId);
            if (existingRatings.Any(r => r.BookId == ratingDto.BookId))
            {
                throw new Exception("User has already rated this book.");
            }

            // Create a new rating
            var rating = new Rating
            {
                BookId = ratingDto.BookId,
                UserId = ratingDto.UserId,
                Score = ratingDto.Score
            };

            return await _ratingRepository.CreateRatingAsync(rating);
        }

        public async Task<List<Rating>> GetRatingsByBookAsync(int bookId)
        {
            return await _ratingRepository.GetRatingsByBookAsync(bookId);
        }

        public async Task<List<Rating>> GetRatingsByUserAsync(int userId)
        {
            return await _ratingRepository.GetRatingsByUserAsync(userId);
        }

        public async Task<Rating?> UpdateRatingAsync(int ratingId, int newScore)
        {
            var rating = await _ratingRepository.GetRatingAsync(ratingId) ?? throw new Exception("Rating not found.");

            // Update the rating score
            rating.Score = newScore;

            return await _ratingRepository.UpdateRatingAsync(rating);
        }

        public async Task<bool> DeleteRatingAsync(int ratingId)
        {
            return await _ratingRepository.DeleteRatingAsync(ratingId);
        }
    }
}