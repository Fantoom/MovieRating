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

        public async Task<List<MovieWithRating>> GetTopMoviesAsync(int count, string? userId = null)
        {
            return await SelectAllMoviesWithRatings(userId).OrderByDescending(x => x.AverageRating).Take(count).ToListAsync();
        }

        public async Task<MovieRatingModel?> GetUserMovieRatingAsync(string userId, int movieId)
        {
            return await _dbContext.MovieRatings.FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
        }

        public async Task<MovieWithRatingAndActors> GetMovieWithRatingAndActorsAsync(int movieId, string? userId = null)
        {
            return await SelectAllMoviesWithRatingsAndActors(userId).FirstAsync(x => x.Movie.Id == movieId);
        }

        private IQueryable<MovieWithRating> SelectAllMoviesWithRatings(string? userId = null, IQueryable<Movie>? movies = null)
        {
            return (movies ?? _dbContext.Movies)
                .Select(x => new MovieWithRating()
                {
                    Movie = new MovieDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        ReleaseDate = x.ReleaseDate
                    },
                    AverageRating = x.Ratings.Average(r => r.Rating),
                    UserRating = MovieRatingDto.MapFrom(x.Ratings.FirstOrDefault(r => r.UserId == userId))
                });
        }

        private IQueryable<MovieWithRatingAndActors> SelectAllMoviesWithRatingsAndActors(string? userId = null, IQueryable<Movie>? movies = null)
        {
            return (movies ?? _dbContext.Movies)
               .Select(x => new MovieWithRatingAndActors()
               {
                   Movie = new MovieDto
                   {
                       Id = x.Id,
                       Title = x.Title,
                       Description = x.Description,
                       ReleaseDate = x.ReleaseDate
                   },
                   AverageRating = x.Ratings.Average(r => r.Rating),
                   UserRating = MovieRatingDto.MapFrom(x.Ratings.FirstOrDefault(r => r.UserId == userId)),
                   Actors = x.Actors.Select(a => new ActorWithRating()
                   {
                       Actor = ActorDto.MapFrom(a),
                       AverageRating = a.Ratings.Average(r => r.Rating),
                       UserRating = a.Ratings.Select(r => new ActorRatingDto { UserId = r.UserId, ActorId = r.ActorId, Rating = r.Rating}).FirstOrDefault(r => r.UserId == userId)
                   }).ToList()
               });
        }
    }
}
