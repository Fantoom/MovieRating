using MovieRating.Dal.Data;
using MovieRating.Dal.Services;
using X.PagedList;
using MovieRating.Dto;
using MovieRating.Dal.Data.Models;

namespace MovieRating.Test
{
    public class ActorServiceTests : IDisposable
    {

        private ApplicationDbContext context;
        private ActorService actorService;
        private List<Actor> generatedActors;
        private static readonly string testUserId = "TestUserId";

        public ActorServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase("ActorTestDB")
                              .Options;
            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();

            generatedActors = GetTestData();
            context.Actors.AddRange(generatedActors);
            context.SaveChanges();
            actorService = new ActorService(context);
        }

        [Theory]
        [InlineData(5, null)]
        [InlineData(5, "TestUserId")]

        public async Task GetTopActorsAsyncTest(int count, string? userId = null)
        {
            var serviceActors = await actorService.GetTopActorsAsync(count, userId) as IEnumerable<ActorWithRatingDto>;
            var expectedActors = generatedActors.SelectAllActorsWithRatings(userId)
                                            .OrderByDescending(x => x.AverageRating)
                                            .Take(count);

            Assert.Equal(expectedActors, serviceActors);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "TestUserId")]
        public async Task GetActorWithRatingAndMoviesAsyncTest(int actorId, string? userId = null)
        {
            var serviceActor = await actorService.GetActorWithRatingAndMoviesAsync(actorId, userId);
            var expectedActor = generatedActors.SelectAllActorsWithRatingsAndMovies(userId)
                                           .First(x => x.Id == actorId);

            Assert.Equal(expectedActor.Id, serviceActor.Id);
            Assert.Equal(expectedActor.Name, serviceActor.Name);
            Assert.Equal(expectedActor.AverageRating, serviceActor.AverageRating);
            Assert.Equal(expectedActor.Movies, serviceActor.Movies);
        }

        [Theory]
        [InlineData("TestUserId", 1)]
        public async Task GetUserActorRatingAsyncTest(string userId, int actorId)
        {
            var serviceActorRating = await actorService.GetUserActorRatingAsync(userId, actorId);
            var expectedRating = generatedActors.First(x => x.Id == actorId).Ratings
                                            .First(x => x.UserId == userId);

            Assert.Equal(expectedRating, serviceActorRating);
        }

        [Theory]
        [InlineData(1, 5, "TestUserId")]
        [InlineData(2, 5, "TestUserId")]
        public async Task GetPagedActorsWithRatingsAsyncTest(int page, int pageSize, string userId)
        {
            var serviceActors = await actorService.GetPagedActorsWithRatingsAsync(page, pageSize, userId) as IEnumerable<ActorRatingDto>;
            var expectedActors = generatedActors.SelectAllActorsWithRatings(userId).ToPagedList(page, pageSize) as IEnumerable<ActorRatingDto>;

            Assert.Equal(expectedActors, serviceActors);
        }

        private static List<Actor> GetTestData()
        {
            var movies = Enumerable.Range(1, 10).Select(movieId => new Movie()
            {
                Title = $"Movie{movieId}",
                Description = $"MovieDescription{movieId}",
                ReleaseDate = DateTime.Parse("30.08.2022"),
                Ratings = Enumerable.Range(1, 5 + movieId).Select(ratingId => new MovieRatingModel
                {
                    Rating = ratingId,
                    UserId = ratingId == 1 ? testUserId : $"userRating{ratingId}"
                }).ToList()
            }).ToList();

            var actors = Enumerable.Range(1, 10).Select(actorId => new Actor()
            {
                Name = $"Actor{actorId}",
                Movies = movies.Take(actorId).ToList(),
                Ratings = Enumerable.Range(1, 5 + actorId).Select(ratingId => new ActorRating
                {
                    Rating = ratingId,
                    UserId = ratingId == 1 ? testUserId : $"userRating{ratingId}"
                }).ToList()
            }).ToList();

            return actors;
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }

    internal static class ActorTestExt
    {
        internal static IEnumerable<ActorWithRatingDto> SelectAllActorsWithRatings(this IEnumerable<Actor> movies, string? userId = null)
        {
            return movies.Select(x => new ActorWithRatingDto()
            {
                Id = x.Id,
                Name = x.Name,
                AverageRating = x.Ratings.Average(r => r.Rating),
                UserRating = x.Ratings.FirstOrDefault(r => r.UserId == userId)?.Rating ?? null
            });
        }
        internal static IEnumerable<ActorWithRatingAndMoviesDto> SelectAllActorsWithRatingsAndMovies(this IEnumerable<Actor> movies, string? userId = null)
        {
            return movies.Select(x => new ActorWithRatingAndMoviesDto()
            {
                Id = x.Id,
                Name = x.Name,
                AverageRating = x.Ratings.Average(r => r.Rating),
                UserRating = x.Ratings.FirstOrDefault(r => r.UserId == userId)?.Rating,
                Movies = x.Movies.Select(m => new MovieWithRatingDto()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    AverageRating = m.Ratings.Average(r => r.Rating),
                    UserRating = m.Ratings.FirstOrDefault(r => r.UserId == userId)?.Rating ?? null
                }).ToList()
            });
        }
    }
}