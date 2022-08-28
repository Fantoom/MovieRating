using MovieRating.Data;
using MovieRating.Data.Models;

namespace MovieRating.Services
{
    public interface IMovieService
    {
        Task AddRatingAsync(string userId, int movieId, int rating);
        Task<List<Movie>> GetAllMoviesAsync();
        Task<MovieWithRatingAndActors> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null);
        Task<AsyncPagedList<MovieWithRating>> GetPagedMoviesWithRatingsAsync(int pageNumber, int pageSize, string? userId = null);
        Task<List<MovieWithRating>> GetTopMoviesAsync(int count);
        Task<MovieRatingModel?> GetUserMovieRatingAsync(string userId, int movieId);
    }
}