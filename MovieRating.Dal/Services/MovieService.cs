using Microsoft.EntityFrameworkCore;
using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;
using MovieRating.Dto;

namespace MovieRating.Dal.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _dbContext;
        public MovieService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AsyncPagedList<MovieWithRatingDto>> GetPagedMoviesWithRatingsAsync(int pageNumber, int pageSize, string? userId = null)
        {
            return await _dbContext.Movies.SelectAllMoviesWithRatings(userId)
                         .ToAsyncPagedList(pageNumber, pageSize);
        }

        public async Task AddRatingAsync(string userId, int movieId, int rating)
        {
            var ratingModel = await InternalGetUserMovieRatingAsync(userId, movieId);
            if (ratingModel is not null)
            {
                ratingModel.Rating = rating;
            }
            else
            {
                ratingModel = new() { MovieId = movieId, Rating = rating, UserId = userId };
            }
            _dbContext.MovieRatings.Update(ratingModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MovieWithRatingDto>> GetTopMoviesAsync(int count, string? userId = null)
        {
            return await _dbContext.Movies.SelectAllMoviesWithRatings(userId)
                .OrderByDescending(x => x.AverageRating)
                .Take(count)
                .ToListAsync();
        }

        public async Task<MovieRatingDto?> GetUserMovieRatingAsync(string userId, int movieId)
        {
            var rating = await InternalGetUserMovieRatingAsync(userId, movieId);

            if (rating is null)
                return null;

            return new MovieRatingDto()
            {
                MovieId = rating.MovieId,
                Rating = rating.Rating,
                UserId = rating.UserId
            };
        }

        public async Task<MovieWithRatingAndActorsDto> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null)
        {
            return await _dbContext.Movies.SelectAllMoviesWithRatingsAndActors(userId).FirstAsync(x => x.Id == movieId);
        }

        private async Task<MovieRatingModel?> InternalGetUserMovieRatingAsync(string userId, int movieId)
        {
            return await _dbContext
                .MovieRatings
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
        }
    }
}
