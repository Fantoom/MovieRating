using MovieRating.Data;
using MovieRating.Data.Models;
using MovieRating.Data.Models.Dtos;

namespace MovieRating.Services
{
    public interface IMovieService
    {
        Task AddRatingAsync(string userId, int movieId, int rating);
        Task<List<Movie>> GetAllMoviesAsync();
        Task<MovieWithRatingAndActorsDto> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null);
        Task<AsyncPagedList<MovieWithRatingDto>> GetPagedMoviesWithRatingsAsync(int pageNumber, int pageSize, string? userId = null);
        Task<List<MovieWithRatingDto>> GetTopMoviesAsync(int count, string? userId = null);
        Task<MovieRatingModel?> GetUserMovieRatingAsync(string userId, int movieId);
    }
}