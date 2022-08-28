using MovieRating.Data;
using MovieRating.Data.Models;

namespace MovieRating.Services
{
    public interface IActorService
    {
        Task AddRatingAsync(string userId, int actorId, int rating);
        Task<ActorWithRatingAndMovies> GetActorWithRatingAndMoviesAsync(int actorId, string? userId = null);
        Task<AsyncPagedList<ActorWithRating>> GetPagedActorsWithRatingsAsync(int pageNumber, int pageSize, string? userId);
        Task<List<ActorWithRating>> GetTopActorsAsync(int count);
        Task<ActorRating?> GetUserActorRatingAsync(string userId, int actorId);
    }
}