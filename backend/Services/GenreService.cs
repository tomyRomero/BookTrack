using backend.Models;
using backend.Models.DTOs;
using backend.Repositories;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;
using System;

namespace backend.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreDTO>> GetAllGenresAsync()
        {
            try
            {
                var genres = await _genreRepository.GetAllGenresAsync();
                return genres.Select(g => new GenreDTO { Name = g.Name }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching genres: {ex.Message}");
            }
        }

        public async Task<GenreDTO?> GetGenreByIdAsync(int genreId)
        {
            try
            {
                var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                if (genre == null) return null;

                return new GenreDTO { Name = genre.Name };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching genre by ID: {ex.Message}");
            }
        }

        public async Task<GenreResponseDTO?> AddGenreAsync(GenreDTO genreDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(genreDTO.Name))
                {
                    throw new ArgumentException("Genre name is required.");
                }

                var genre = new Genre { Name = genreDTO.Name };
                var addedGenre = await _genreRepository.AddGenreAsync(genre);

                return new GenreResponseDTO
                {
                    GenreId = addedGenre.GenreId,
                    Name = addedGenre.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding genre: {ex.Message}");
            }
        }

        public async Task<GenreResponseDTO?> UpdateGenreAsync(int genreId, GenreDTO genreDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(genreDTO.Name) || genreDTO.Name.Length > 100)
                {
                    return null; 
                }

                var genre = await _genreRepository.GetGenreByIdAsync(genreId);

                if (genre == null)
                {
                    return null; 
                }

                genre.Name = genreDTO.Name;
                var updatedGenre = await _genreRepository.UpdateGenreAsync(genre);

                return new GenreResponseDTO
                {
                    GenreId = updatedGenre.GenreId,
                    Name = updatedGenre.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating genre: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteGenreAsync(int genreId)
        {
            try
            {
                return await _genreRepository.DeleteGenreAsync(genreId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting genre: {ex.Message}");
            }
        }
    }
}