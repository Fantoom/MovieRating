using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;
using MovieRating.Dto;

namespace MovieRating.Dal.Services
{
    public interface IActorService
    {
        Task AddRatingAsync(string userId, int actorId, int rating);
        Task<ActorWithRatingAndMoviesDto> GetActorWithRatingAndMoviesAsync(int actorId, string? userId = null);
        Task<AsyncPagedList<ActorWithRatingDto>> GetPagedActorsWithRatingsAsync(int pageNumber, int pageSize, string? userId);
        Task<List<ActorWithRatingDto>> GetTopActorsAsync(int count, string? userId);
        Task<ActorRating?> GetUserActorRatingAsync(string userId, int actorId);
    }
}