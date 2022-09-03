using MovieRating.Dal.Data;
using MovieRating.Dal.Data.Models;
using MovieRating.Dal.Services;
using MovieRating.Dto;
using X.PagedList;

namespace MovieRating.Test
{
    [UsesVerify]
    public class MovieServiceTests : IDisposable
    {

        private ApplicationDbContext context;
        private MovieService movieService;
        private List<Movie> generatedMovies;
        private VerifySettings settings;
        private static readonly string testUserId = "TestUserId";

        public MovieServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase("MovieTestDB")
                              .Options;

            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();

            generatedMovies = GetTestData();
            context.Movies.AddRange(generatedMovies);
            context.SaveChanges();
            movieService = new MovieService(context);
            settings = GlobalSetups.GetMovieVerifySettings();
        }

        [Theory]
        [InlineData(5, null)]
        [InlineData(5, "TestUserId")]

        public async Task GetTopMoviesAsyncTest(int count, string? userId = null)
        {
            var serviceMovies = await movieService.GetTopMoviesAsync(count, userId);
            await Verify(serviceMovies, settings).UseParameters(count, userId);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "TestUserId")]
        public async Task GetMovieWithRatingAndActorsAsyncTest(int movieId, string? userId = null)
        {
            var serviceMovie = await movieService.GetMovieWithRatingAndActorsAsync(movieId, userId);
            await Verify(serviceMovie, settings).UseParameters(movieId, userId);
        }

        [Theory]
        [InlineData("TestUserId", 1)]
        public async Task GetUserMovieRatingAsyncTest(string userId, int movieId)
        {
            var serviceMovieRating = await movieService.GetUserMovieRatingAsync(userId, movieId);
            await Verify(serviceMovieRating, settings).UseParameters(userId, movieId);
        }

        [Theory]
        [InlineData(1, 5, "TestUserId")]
        [InlineData(2, 5, "TestUserId")]
        public async Task GetPagedMoviesWithRatingsAsyncTest(int page, int pageSize, string userId)
        {
            var serviceMovies = await movieService.GetPagedMoviesWithRatingsAsync(page, pageSize, userId);
            await Verify(serviceMovies, settings).UseParameters(page, pageSize, userId);
        }

        private static List<Movie> GetTestData()
        {
            var actors = Enumerable.Range(1, 10).Select(actorId => new Actor()
            {
                Name = $"Actor{actorId}",
                Ratings = Enumerable.Range(1, 5).Select(ratingId => new ActorRating
                {
                    Rating = ratingId,
                    UserId = ratingId == 1 ? testUserId : $"userRating{ratingId}"
                }).ToList()
            }).ToList();

            return Enumerable.Range(1, 10).Select(movieId => new Movie()
            {
                Title = $"Movie{movieId}",
                Description = $"MovieDescription{movieId}",
                ReleaseDate = DateTime.Parse("30.08.2022"),
                Actors = actors.Take(movieId).ToList(),
                Ratings = Enumerable.Range(1, 5).Select(ratingId => new MovieRatingModel
                {
                    Rating = ratingId,
                    UserId = ratingId == 1 ? testUserId : $"userRating{ratingId}"
                }).ToList()
            }).ToList();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}