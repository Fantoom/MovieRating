using Microsoft.EntityFrameworkCore;
using MovieRating.Data;
using MovieRating.Data.Models;

namespace MovieRating.Services
{
    public class ActorService : IActorService
    {
        private readonly ApplicationDbContext _dbContext;
        public ActorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<ActorWithRating> SelectAllActorsWithRatings(string? userId = null, IQueryable<Actor>? actors = null)
        {
            return (actors ?? _dbContext.Actors)
               .Select(x => new ActorWithRating()
               {
                   Actor = ActorDto.MapFrom(x),
                   AverageRating = x.Ratings.Average(r => r.Rating),
                   UserRating = ActorRatingDto.MapFrom(x.Ratings.FirstOrDefault(r => r.UserId == userId))
               });
        }

        private IQueryable<ActorWithRatingAndMovies> SelectAllActorsWithRatingsAndMovies(string? userId = null, IQueryable<Actor>? actors = null)
        {
            return (actors ?? _dbContext.Actors)
               .Select(actor => new ActorWithRatingAndMovies()
               {
                   Actor = new ActorDto
                   {
                       Id = actor.Id,
                       Name = actor.Name
                   },
                   AverageRating = actor.Ratings.Average(r => r.Rating),
                   UserRating = ActorRatingDto.MapFrom(actor.Ratings.FirstOrDefault(r => r.UserId == userId)),
                   Movies = actor.Movies.Select(movie => new MovieWithRating()
                   {
                       Movie = new MovieDto
                       {
                           Id = movie.Id,
                           Title = movie.Title,
                           Description = movie.Description,
                           ReleaseDate = movie.ReleaseDate
                       },
                       AverageRating = movie.Ratings.Average(r => r.Rating),
                       UserRating = MovieRatingDto.MapFrom(movie.Ratings.FirstOrDefault(r => r.UserId == userId))
                   }).ToList()
               }); ;
        }

        public async Task<AsyncPagedList<ActorWithRating>> GetPagedActorsWithRatingsAsync(int pageNumber, int pageSize, string? userId)
        {
            return await SelectAllActorsWithRatings(userId).ToAsyncPagedList(pageNumber, pageSize);
        }

        public async Task<List<ActorWithRating>> GetTopActorsAsync(int count)
        {
            return await SelectAllActorsWithRatings().OrderByDescending(x => x.AverageRating).Take(count).ToListAsync();
        }

        public async Task<ActorRating?> GetUserActorRatingAsync(string userId, int actorId)
        {
            return await _dbContext.ActorRatings.FirstOrDefaultAsync(x => x.UserId == userId && x.ActorId == actorId);
        }

        public async Task<ActorWithRatingAndMovies> GetActorWithRatingAndMoviesAsync(int actorId, string? userId = null)
        {
            return await SelectAllActorsWithRatingsAndMovies(userId).FirstAsync(x => x.Actor.Id == actorId);
        }

        public async Task AddRatingAsync(string userId, int actorId, int rating)
        {
            var ratingModel = await GetUserActorRatingAsync(userId, actorId);
            if (ratingModel is not null)
            {
                ratingModel.Rating = rating;
            }
            else
            {
                ratingModel = new() { ActorId = actorId, Rating = rating, UserId = userId };
            }
            _dbContext.ActorRatings.Update(ratingModel);
            await _dbContext.SaveChangesAsync();
        }
    }
}
