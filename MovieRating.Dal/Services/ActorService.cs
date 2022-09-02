using Microsoft.EntityFrameworkCore;
using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;
using MovieRating.Dto;

namespace MovieRating.Dal.Services
{
    public class ActorService : IActorService
    {
        private readonly ApplicationDbContext _dbContext;
        public ActorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AsyncPagedList<ActorWithRatingDto>> GetPagedActorsWithRatingsAsync(int pageNumber, int pageSize, string? userId)
        {
            return await SelectAllActorsWithRatings(userId)
                .ToAsyncPagedList(pageNumber, pageSize);
        }

        public async Task<List<ActorWithRatingDto>> GetTopActorsAsync(int count, string? userId)
        {
            return await SelectAllActorsWithRatings(userId)
                .OrderByDescending(x => x.AverageRating)
                .Take(count).ToListAsync();
        }

        public async Task<ActorRatingDto?> GetUserActorRatingAsync(string userId, int actorId)
        {
            var rating = await InternalGetUserActorRatingAsync(userId, actorId);

            if (rating is null)
                return null;

            return new ActorRatingDto()
            {
                ActorId = rating.ActorId,
                Rating = rating.Rating,
                UserId = rating.UserId
            };
        }

        public async Task<ActorWithRatingAndMoviesDto> GetActorWithRatingAndMoviesAsync(int actorId, string? userId = null)
        {
            return await SelectAllActorsWithRatingsAndMovies(userId)
                .FirstAsync(x => x.Id == actorId);
        }

        public async Task AddRatingAsync(string userId, int actorId, int rating)
        {
            var ratingModel = await InternalGetUserActorRatingAsync(userId, actorId);
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

        public async Task<ActorRating?> InternalGetUserActorRatingAsync(string userId, int actorId)
        {
            return await _dbContext
                .ActorRatings
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ActorId == actorId);
        }


        private IQueryable<ActorWithRatingDto> SelectAllActorsWithRatings(string? userId = null, IQueryable<Actor>? actors = null)
        {
            return (actors ?? _dbContext.Actors)
               .Select(actor => new ActorWithRatingDto()
               {
                   Id = actor.Id,
                   Name = actor.Name,
                   AverageRating = actor.Ratings.Average(r => r.Rating),
                   UserRating = actor.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault()
               });
        }

        private IQueryable<ActorWithRatingAndMoviesDto> SelectAllActorsWithRatingsAndMovies(string? userId = null, IQueryable<Actor>? actors = null)
        {
            return (actors ?? _dbContext.Actors)
               .Select(actor => new ActorWithRatingAndMoviesDto()
               {

                   Id = actor.Id,
                   Name = actor.Name,
                   AverageRating = actor.Ratings.Average(r => r.Rating),
                   UserRating = actor.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault(),
                   Movies = actor.Movies.Select(movie => new MovieWithRatingDto()
                   {

                       Id = movie.Id,
                       Title = movie.Title,
                       Description = movie.Description,
                       ReleaseDate = movie.ReleaseDate,
                       AverageRating = movie.Ratings.Average(r => r.Rating),
                       UserRating = movie.Ratings.Where(r => r.UserId == userId).Select(r => r.Rating as int?).FirstOrDefault()
                   }).ToList()
               });
        }
    }
}
