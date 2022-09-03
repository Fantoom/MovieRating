using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;
using MovieRating.Dal.Services;
using MovieRating.Dto;
using X.PagedList;

namespace MovieRating.Test
{
    [UsesVerify]
    public class ActorServiceTests : IDisposable
    {

        private ApplicationDbContext context;
        private ActorService actorService;
        private List<Actor> generatedActors;
        private VerifySettings settings;
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
            settings = GlobalSetups.GetActorVerifySettings();

        }

        [Theory]
        [InlineData(5, null)]
        [InlineData(5, "TestUserId")]

        public async Task GetTopActorsAsyncTest(int count, string? userId = null)
        {
            var serviceActors = await actorService.GetTopActorsAsync(count, userId);
            await Verify(serviceActors, settings).UseParameters(count, userId);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "TestUserId")]
        public async Task GetActorWithRatingAndMoviesAsyncTest(int actorId, string? userId = null)
        {
            var serviceActor = await actorService.GetActorWithRatingAndMoviesAsync(actorId, userId);
            await Verify(serviceActor, settings).UseParameters(actorId, userId);
        }

        [Theory]
        [InlineData("TestUserId", 1)]
        public async Task GetUserActorRatingAsyncTest(string userId, int actorId)
        {
            var serviceActorRating = await actorService.GetUserActorRatingAsync(userId, actorId);
            await Verify(serviceActorRating, settings).UseParameters(userId, actorId);
        }

        [Theory]
        [InlineData(1, 5, "TestUserId")]
        [InlineData(2, 5, "TestUserId")]
        public async Task GetPagedActorsWithRatingsAsyncTest(int page, int pageSize, string userId)
        {
            var serviceActors = await actorService.GetPagedActorsWithRatingsAsync(page, pageSize, userId);
            await Verify(serviceActors, settings).UseParameters(page, pageSize, userId);
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
}