using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;

namespace MovieRating
{
    public class InitialData
    {

        // This method is added and used only for development and testing
        public static async Task InsertFakeData(ApplicationDbContext dbContext)
        {
            var actors = Enumerable.Range(1, 10).Select(actorId => new Actor()
            {
                Name = $"Actor{Random.Shared.Next(actorId * 10000, (actorId + 1) * 10000)}",
            }).ToList();

            var movies = Enumerable.Range(1, 10).Select(movieId => new Movie()
            {
                Title = $"Movie{Random.Shared.Next(movieId * 10000, (movieId + 1) * 10000)}",
                Description = $"MovieDescription{movieId}",
                ReleaseDate = DateTime.Parse("30.08.2022"),
                Actors = actors.Take(movieId).ToList(),
            }).ToList();

            await dbContext.Movies.AddRangeAsync(movies);
            await dbContext.SaveChangesAsync();
        }
    }
}
