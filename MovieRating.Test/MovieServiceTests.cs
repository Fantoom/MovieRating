using MovieRating.Data;
using MovieRating.Data.Models;
using MovieRating.Services;
using X.PagedList;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieRating.Test
{
    public class MovieServiceTests : IDisposable
    {

        private ApplicationDbContext context;
        private MovieService movieService;
        private List<Movie> generatedMovies;
        private static readonly string testUserId = "TestUserId";

        public MovieServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase("TestDB")
                              .Options;
            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();

            generatedMovies = GetTestData();
            context.Movies.AddRange(generatedMovies);
            context.SaveChanges();
            movieService = new MovieService(context);
        }

        private List<Movie> GetTestData()
        {
            var Actors = Enumerable.Range(1, 10).Select(actorId => new Actor()
            {
                Id = actorId,
                Name = $"Actor{actorId}",
                Ratings = Enumerable.Range(1, 5).Select(ratingId => new ActorRating
                {
                    Id = actorId * 1000 + ratingId,
                    ActorId = actorId,
                    Rating = ratingId,
                    UserId = testUserId
                }).ToList()
            }).ToList();

            return Enumerable.Range(1, 10).Select(movieId => new Movie()
            {
                Id = movieId,
                Title = $"Movie{movieId}",
                Description = $"MovieDescription{movieId}",
                ReleaseDate = DateTime.Parse("30.08.2022"),
                Actors = Actors.Take(movieId).ToList(),
                Ratings = Enumerable.Range(1, 5).Select(ratingId => new MovieRatingModel
                {
                    Id = movieId * 1000 + ratingId,
                    MovieId = movieId,
                    Rating = ratingId,
                    UserId = testUserId
                }).ToList()
            }).ToList();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        [Fact]
        public async Task GetAllMoviesAsyncTest()
        {
            var serviceMovies = await movieService.GetAllMoviesAsync();
            Assert.Equal(serviceMovies, generatedMovies);
        }

        [Theory]
        [InlineData(5, null)]
        [InlineData(5, "TestUserId")]

        public async Task GetTopMoviesAsyncTest(int count, string? userId = null)
        {
            var serviceMovies = await movieService.GetTopMoviesAsync(count, userId);
            var expectedMovies = generatedMovies.SelectAllMoviesWithRatings(userId)
                                            .OrderByDescending(x => x.AverageRating)
                                            .Take(count);
            //var expected = JsonSerializer.Serialize(expectedMovies, new JsonSerializerOptions() { MaxDepth = 5, ReferenceHandler = ReferenceHandler.IgnoreCycles });;
            //var got = JsonSerializer.Serialize(serviceMovies, new JsonSerializerOptions() { MaxDepth = 5, ReferenceHandler = ReferenceHandler.IgnoreCycles });

            Assert.Equal(serviceMovies, expectedMovies);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(1, "TestUserId")]
        public async Task GetMovieWithRatingAndActorsAsyncTest(int movieId, string? userId = null)
        {
            var serviceMovie = await movieService.GetMovieWithRatingAndActorsAsync(movieId, userId);
            var expectedMovie = generatedMovies.SelectAllMoviesWithRatingsAndActors(userId)
                                           .First(x => x.Movie.Id == movieId);

            Assert.Equal(serviceMovie.UserRating, expectedMovie.UserRating);
        }

        [Theory]
        [InlineData("TestUserId", 1)]
        public async Task GetUserMovieRatingAsyncTest(string userId, int movieId)
        {
            var movieRating = await movieService.GetUserMovieRatingAsync(userId, movieId);
            var testRating = generatedMovies.First(x => x.Id == movieId).Ratings
                                            .First(x => x.UserId == userId);

            Assert.Equal(movieRating, testRating);
        }

        [Theory]
        [InlineData(1, 5, "TestUserId")]
        [InlineData(2, 5, "TestUserId")]
        public async Task GetPagedMoviesWithRatingsAsyncTest(int page, int pageSize, string userId)
        {
            var movies = await movieService.GetPagedMoviesWithRatingsAsync(page, pageSize, userId) as IEnumerable<MovieWithRating>;
            var testMovies = generatedMovies.SelectAllMoviesWithRatings(userId).ToPagedList(page, pageSize) as IEnumerable<MovieWithRating>;

            Assert.Equal(movies, testMovies);
        }

    }

    internal static class MovieTestExt
    {
        internal static IEnumerable<MovieWithRating> SelectAllMoviesWithRatings(this IEnumerable<Movie> movies, string? userId = null)
        {
            return movies.Select(x => new MovieWithRating()
            {
                Movie = MovieDto.MapFrom(x),
                AverageRating = x.Ratings.Average(r => r.Rating),
                UserRating = MovieRatingDto.MapFrom(x.Ratings.FirstOrDefault(r => r.UserId == userId))
            });
        }
        internal static IEnumerable<MovieWithRating> SelectAllMoviesWithRatingsAndActors(this IEnumerable<Movie> movies, string? userId = null)
        {
            return movies.Select(x => new MovieWithRatingAndActors()
            {
                Movie = MovieDto.MapFrom(x),
                AverageRating = x.Ratings.Average(r => r.Rating),
                UserRating = MovieRatingDto.MapFrom(x.Ratings.FirstOrDefault(r => r.UserId == userId)),
                Actors = x.Actors.Select(a => new ActorWithRating()
                {
                    Actor = ActorDto.MapFrom(a),
                    AverageRating = a.Ratings.Average(r => r.Rating),
                    UserRating = ActorRatingDto.MapFrom(a.Ratings.FirstOrDefault(r => r.UserId == userId))
                }).ToList()
            });
        }
    }
}