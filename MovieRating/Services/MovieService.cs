using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieRating.Data;
using MovieRating.Data.Models;
using MovieRating.Data.Models.Dtos;

namespace MovieRating.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _dbContext;
        public MovieService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _dbContext.Movies.ToListAsync();
        }

        public async Task<AsyncPagedList<MovieWithRatingDto>> GetPagedMoviesWithRatingsAsync(int pageNumber, int pageSize, string? userId = null)
        {
            return await SelectAllMoviesWithRatings(userId)
                         .ToAsyncPagedList(pageNumber, pageSize);
        }

        public async Task AddRatingAsync(string userId, int movieId, int rating)
        {
            var ratingModel = await GetUserMovieRatingAsync(userId, movieId);
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
            return await SelectAllMoviesWithRatings(userId)
                .OrderByDescending(x => x.AverageRating)
                .Take(count)
                .ToListAsync();
        }

        public async Task<MovieRatingModel?> GetUserMovieRatingAsync(string userId, int movieId)
        {
            return await _dbContext
                .MovieRatings
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
        }

        public async Task<MovieWithRatingAndActorsDto> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null)
        {
            return await SelectAllMoviesWithRatingsAndActors(userId).FirstAsync(x => x.Id == movieId);
        }

        private IQueryable<MovieWithRatingDto> SelectAllMoviesWithRatings(string? userId = null, IQueryable<Movie>? movies = null)
        {
            return (movies ?? _dbContext.Movies)
                .Select(x => new MovieWithRatingDto()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ReleaseDate = x.ReleaseDate,
                    AverageRating = x.Ratings.Average(r => r.Rating),
                    UserRating = x.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating).FirstOrDefault()
                });
        }

        private IQueryable<MovieWithRatingAndActorsDto> SelectAllMoviesWithRatingsAndActors(string? userId = null, IQueryable<Movie>? movies = null)
        {
            return (movies ?? _dbContext.Movies)
                .Select(x => new MovieWithRatingAndActorsDto()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ReleaseDate = x.ReleaseDate,
                    AverageRating = x.Ratings.Average(r => r.Rating),
                    UserRating = x.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating).FirstOrDefault(),
                    Actors = x.Actors.Select(a => new ActorWithRatingDto()
                    {
                        Id = a.Id,
                        Name = a.Name,
                        AverageRating = a.Ratings.Average(r => r.Rating),
                        UserRating = a.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating).FirstOrDefault()
                    }).ToList()
                });
        }
    }
}
