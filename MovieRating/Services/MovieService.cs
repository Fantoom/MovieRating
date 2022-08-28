using Microsoft.EntityFrameworkCore;
using MovieRating.Data;
using MovieRating.Data.Models;

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

        private IQueryable<MovieWithRating> SelectAllMoviesWithRatings(string? userId = null, IQueryable<Movie>? movies = null)
        {
            //return _dbContext.Movies.Join(_dbContext.Ratings, x => x.Id, x => x.MovieId, (x, y) => y);
            //return _dbContext.MoviesWithRating.FromSqlInterpolated($"Select Movies.*, ( Select AVG(Cast(Rating as float)) from Ratings Inner JOIN Movies On Movies.Id = Ratings.MovieId ) as Rating from Movies");
            return (movies ?? _dbContext.Movies)
                .Select(x => new MovieWithRating()
                {
                    Movie = x,
                    AverageRating = x.Ratings.Average(r => r.Rating),
                    UserRating = _dbContext.MovieRatings.FirstOrDefault(r => r.UserId == userId)
                });
        }

        private IQueryable<MovieWithRatingAndActors> SelectAllMoviesWithRatingsAndActors(string? userId = null, IQueryable<Movie>? movies = null)
        {
            return (movies ?? _dbContext.Movies)
               .Select(x => new MovieWithRatingAndActors()
               {
                   Movie = x,
                   AverageRating = x.Ratings.Average(r => r.Rating),
                   UserRating = _dbContext.MovieRatings.FirstOrDefault(r => r.UserId == userId),
                   Actors = _dbContext.Actors.Select(a => new ActorWithRating()
                   {
                       Actor = a,
                       AverageRating = a.Ratings.Average(r => r.Rating),
                       UserRating = _dbContext.ActorRatings.FirstOrDefault(r => r.UserId == userId)
                   }).ToList()
               }); ;
        }

        public async Task<AsyncPagedList<MovieWithRating>> GetPagedMoviesWithRatingsAsync(int pageNumber, int pageSize, string? userId = null)
        {
            return await SelectAllMoviesWithRatings(userId).ToAsyncPagedList(pageNumber, pageSize);
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

        public async Task<List<MovieWithRating>> GetTopMoviesAsync(int count)
        {
            return await SelectAllMoviesWithRatings().OrderByDescending(x => x.AverageRating).Take(count).ToListAsync();
        }

        public async Task<MovieRatingModel?> GetUserMovieRatingAsync(string userId, int movieId)
        {
            return await _dbContext.MovieRatings.FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
        }

        public async Task<MovieWithRatingAndActors> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null)
        {
            return await SelectAllMoviesWithRatingsAndActors(userId).FirstAsync(x => x.Movie.Id == movieId);
        }
    }
}
